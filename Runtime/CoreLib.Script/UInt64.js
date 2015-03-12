////////////////////////////////////////////////////////////////////////////////
// ss.UInt64
var $ss_UInt64 = function (low, mid, high) {
    this.$low = 0;
    this.$mid = 0;
    this.$high = 0;
    this.$low = low & 16777215;
    this.$mid = mid & 16777215;
    this.$high = high & 65535;
};
$ss_UInt64.__typeName = 'ss.UInt64';
$ss_UInt64.createInstance = function () {
    return ss.Int32.Zero;
};
$ss_UInt64.getDefaultValue = $ss_UInt64.createInstance;
$ss_UInt64.parse = function (text) {
    var result = {};
    if (!$ss_UInt64.tryParse(text, result)) {
        throw new ss.FormatException('Input string was not in a correct format.');
    }
    return result.$;
};
$ss_UInt64.tryParse = function (text, result) {
    var radix = 10;
    result.$ = $ss_UInt64.zero;
    //if (style & System.Globalization.NumberStyles.AllowHexSpecifier)
    //    radix = 16;
    var rdx = new $ss_UInt64(radix, 0, 0);
    for (var i = 0; i < text.length; i++) {
        if (i === 0 && text.charCodeAt(i) === 45) {
            result.$ = $ss_UInt64.zero;
            return false;
        }
        var c = parseInt(String.fromCharCode(text.charCodeAt(i)), radix);
        if (isNaN(c)) {
            result.$ = $ss_UInt64.zero;
            return false;
        }
        result.$ = $ss_UInt64.op_Addition(new $ss_UInt64(c, 0, 0), $ss_UInt64.op_Multiply(rdx, result.$));
    }
    return true;
};
$ss_UInt64.op_Addition = function (a, b) {
    //same as Int64
    var cLow = a.$low + b.$low;
    var rLow = (cLow & $ss_UInt64.$mask) >> 24;
    var cMid = rLow + a.$mid + b.$mid;
    var rMid = (cMid & $ss_UInt64.$mask) >> 24;
    var cHigh = rMid + a.$high + b.$high;
    return new $ss_UInt64(cLow, cMid, cHigh);
};
$ss_UInt64.op_Subtraction = function (a, b) {
    //same as Int64
    var cLow = a.$low - b.$low | 0;
    var rLow = 0;
    if (cLow < 0) {
        cLow = 16777216 + cLow;
        rLow = -1;
    }
    var cMid = rLow + (a.$mid - b.$mid | 0) | 0;
    var rMid = 0;
    if (cMid < 0) {
        cMid = 16777216 + cMid;
        rMid = -1;
    }
    var cHigh = rMid + (a.$high - b.$high | 0) | 0;
    if (cHigh < 0) {
        cHigh = 65536 + cHigh;
    }
    return new $ss_UInt64(cLow, cMid, cHigh);
};
$ss_UInt64.op_Multiply = function (a, b) {
    if (a.equalsT($ss_UInt64.zero) || b.equalsT($ss_UInt64.zero)) {
        return $ss_UInt64.zero;
    }
    if ($ss_UInt64.op_GreaterThan(a, b)) {
        return $ss_UInt64.op_Multiply(b, a);
    }
    var c = $ss_UInt64.zero;
    if ((a.$low & 1) === 1) {
        c = b;
    }
    while ($ss_UInt64.op_Inequality(a, $ss_UInt64.one)) {
        a = $ss_UInt64.op_RightShift(a, 1);
        b = $ss_UInt64.op_LeftShift(b, 1);
        if ((a.$low & 1) === 1) {
            c = $ss_UInt64.op_Addition(c, b);
        }
    }
    return c;
};
$ss_UInt64.op_Division = function (a, b) {
    if (b.equalsT($ss_UInt64.minValue)) {
        throw new ss.DivideByZeroException();
    }
    var q = $ss_UInt64.zero;
    var r = $ss_UInt64.zero;
    for (var i = 63; i >= 0; i--) {
        r = $ss_UInt64.op_LeftShift(r, 1);
        var x;
        var s;
        if (i < 24) {
            x = a.$low;
            s = i;
        }
        else if (i < 48) {
            x = a.$mid;
            s = i - 24;
        }
        else {
            x = a.$high;
            s = i - 48;
        }
        r = new $ss_UInt64(r.$low | (x & 1 << s) >> s, r.$mid, r.$high);
        if ($ss_UInt64.op_LessThan(r, b)) {
            continue;
        }
        r = $ss_UInt64.op_Subtraction(r, b);
        if (i < 24) {
            q = new $ss_UInt64(q.$low | 1 << s, q.$mid, q.$high);
        }
        else if (i < 48) {
            q = new $ss_UInt64(q.$low, q.$mid | 1 << s, q.$high);
        }
        else {
            q = new $ss_UInt64(q.$low, q.$mid, q.$high | 1 << s);
        }
    }
    return q;
};
$ss_UInt64.op_Modulus = function (a, b) {
    if (b.equalsT($ss_UInt64.minValue)) {
        throw new ss.DivideByZeroException();
    }
    var r = $ss_UInt64.zero;
    for (var i = 63; i >= 0; i--) {
        r = $ss_UInt64.op_LeftShift(r, 1);
        var x;
        var s;
        if (i < 24) {
            x = a.$low;
            s = i;
        }
        else if (i < 48) {
            x = a.$mid;
            s = i - 24;
        }
        else {
            x = a.$high;
            s = i - 48;
        }
        r = new $ss_UInt64(r.$low | (x & 1 << s) >> s, r.$mid, r.$high);
        if ($ss_UInt64.op_GreaterThanOrEqual(r, b)) {
            r = $ss_UInt64.op_Subtraction(r, b);
        }
    }
    return r;
};
$ss_UInt64.op_BitwiseAnd = function (a, b) {
    //same as Int64
    return new $ss_UInt64(a.$low & b.$low, a.$mid & b.$mid, a.$high & b.$high);
};
$ss_UInt64.op_BitwiseOr = function (a, b) {
    //same as Int64
    return new $ss_UInt64(a.$low | b.$low, a.$mid | b.$mid, a.$high | b.$high);
};
$ss_UInt64.op_ExclusiveOr = function (a, b) {
    //same as Int64
    return new $ss_UInt64(a.$low ^ b.$low, a.$mid ^ b.$mid, a.$high ^ b.$high);
};
$ss_UInt64.op_LeftShift = function (a, b) {
    //same as Int64
    b = b & 63;
    var maxShift = 8;
    if (b > 8) {
        return $ss_UInt64.op_LeftShift($ss_UInt64.op_LeftShift(a, maxShift), b - maxShift);
    }
    var cLowT = a.$low << b;
    var cLow = cLowT & 16777215;
    var rLow = cLowT >>> 24 & 16777215;
    var cMidT = a.$mid << b | rLow;
    var cMid = cMidT & 16777215;
    var rMid = cMidT >>> 24 & 65535;
    var cHighT = a.$high << b;
    var cHigh = cHighT & 65535 | rMid;
    return new $ss_UInt64(cLow, cMid, cHigh);
};
$ss_UInt64.op_RightShift = function (a, b) {
    b = b & 63;
    if (b > 24) {
        return $ss_UInt64.op_RightShift($ss_UInt64.op_RightShift(a, 24), b - 24);
    }
    var m = (1 << b) - 1;
    var rHigh = (a.$high & m) << 24 - b;
    var cHighT = a.$high >> b;
    var rMid = (a.$mid & m) << 24 - b;
    var cMidT = a.$mid >> b;
    var cLowT = a.$low >> b;
    return new $ss_UInt64(cLowT | rMid, cMidT | rHigh, cHighT);
};
$ss_UInt64.op_Equality = function (a, b) {
    //same as Int64
    return a.$low === b.$low && a.$mid === b.$mid && a.$high === b.$high;
};
$ss_UInt64.op_Inequality = function (a, b) {
    //same as Int64
    return a.$low !== b.$low || a.$mid !== b.$mid || a.$high !== b.$high;
};
$ss_UInt64.op_LessThanOrEqual = function (a, b) {
    var adiff = a.$high - b.$high;
    if (adiff < 0) {
        return true;
    }
    if (adiff > 0) {
        return false;
    }
    var bdiff = a.$mid - b.$mid;
    if (bdiff < 0) {
        return true;
    }
    if (bdiff > 0) {
        return false;
    }
    return a.$low <= b.$low;
};
$ss_UInt64.op_GreaterThanOrEqual = function (a, b) {
    var adiff = a.$high - b.$high;
    if (adiff > 0) {
        return true;
    }
    if (adiff < 0) {
        return false;
    }
    var bdiff = a.$mid - b.$mid;
    if (bdiff > 0) {
        return true;
    }
    if (bdiff < 0) {
        return false;
    }
    return a.$low >= b.$low;
};
$ss_UInt64.op_LessThan = function (a, b) {
    var adiff = a.$high - b.$high;
    if (adiff < 0) {
        return true;
    }
    if (adiff > 0) {
        return false;
    }
    var bdiff = a.$mid - b.$mid;
    if (bdiff < 0) {
        return true;
    }
    if (bdiff > 0) {
        return false;
    }
    return a.$low < b.$low;
};
$ss_UInt64.op_GreaterThan = function (a, b) {
    var adiff = a.$high - b.$high;
    if (adiff > 0) {
        return true;
    }
    if (adiff < 0) {
        return false;
    }
    var bdiff = a.$mid - b.$mid;
    if (bdiff > 0) {
        return true;
    }
    if (bdiff < 0) {
        return false;
    }
    return a.$low > b.$low;
};
$ss_UInt64.op_UnaryNegation = function (a) {
    return $ss_Int64.op_Addition($ss_Int64.op_Explicit$6($ss_UInt64.op_OnesComplement(a)), $ss_Int64.one);
};
$ss_UInt64.op_OnesComplement = function (a) {
    return new $ss_UInt64(~a.$low, ~a.$mid, ~a.$high);
};
$ss_UInt64.op_Increment = function (a) {
    //same as Int64
    var cLow = a.$low + 1;
    var rLow = (cLow & $ss_UInt64.$mask) >> 24;
    var cMid = rLow + a.$mid;
    var rMid = (cMid & $ss_UInt64.$mask) >> 24;
    var cHigh = rMid + a.$high;
    return new $ss_UInt64(cLow, cMid, cHigh);
};
$ss_UInt64.op_Decrement = function (a) {
    //same as Int64
    var cLow = a.$low - 1 | 0;
    var rLow = 0;
    if (cLow < 0) {
        cLow = 16777216 + cLow;
        rLow = -1;
    }
    var cMid = rLow + a.$mid | 0;
    var rMid = 0;
    if (cMid < 0) {
        cMid = 16777216 + cMid;
        rMid = -1;
    }
    var cHigh = rMid + a.$high | 0;
    if (cHigh < 0) {
        cHigh = 65536 + cHigh;
    }
    return new $ss_UInt64(cLow, cMid, cHigh);
};
$ss_UInt64.op_Explicit$3 = function (a) {
    return new $ss_UInt64(a.$low, a.$mid, a.$high);
};
$ss_UInt64.op_Implicit = function (a) {
    return new $ss_UInt64(a, 0, 0);
};
$ss_UInt64.op_Implicit$3 = function (a) {
    return new $ss_UInt64(a, ((a < 0) ? 16777215 : 0), ((a < 0) ? 65535 : 0));
};
$ss_UInt64.op_Implicit$4 = function (a) {
    return new $ss_UInt64(a, 0, 0);
};
$ss_UInt64.op_Implicit$1 = function (a) {
    return new $ss_UInt64(a, ((a < 0) ? 16777215 : 0), ((a < 0) ? 65535 : 0));
};
$ss_UInt64.op_Implicit$5 = function (a) {
    return new $ss_UInt64(a, a >>> 24, 0);
};
$ss_UInt64.op_Implicit$2 = function (a) {
    return new $ss_UInt64(a, a >> 24, ((a < 0) ? 65535 : 0));
};
$ss_UInt64.op_Explicit$1 = function (a) {
    if (a < 0) {
        throw new ss.ArgumentOutOfRangeException();
    }
    var floorN = Math.floor(a);
    var n0 = ss.Int32.trunc(floorN) | 0;
    var n1 = ss.Int32.trunc(floorN / 16777216) | 0;
    var n2 = ss.Int32.trunc(floorN / 281474976710656) | 0;
    return new $ss_UInt64(n0, n1, n2);
};
$ss_UInt64.op_Explicit$2 = function (a) {
    return $ss_UInt64.op_Explicit$1(a);
};
$ss_UInt64.op_Explicit = function (a) {
    return $ss_UInt64.op_Explicit$1(a);
};
$ss_UInt64.op_Implicit$6 = function (a) {
    return a.$low & 255;
};
$ss_UInt64.op_Implicit$9 = function (a) {
    return a.$low & 255;
};
$ss_UInt64.op_Implicit$a = function (a) {
    return a.$low & 65535;
};
$ss_UInt64.op_Implicit$7 = function (a) {
    return a.$low & 65535;
};
$ss_UInt64.op_Implicit$b = function (a) {
    //return (UInt32)((a.Low | a.Mid << 24) & UInt32.MaxValue);
    // return (a.$low | a.$mid << 24) & 4294967295;
    throw new ss.NotImplementedException();
};
$ss_UInt64.op_Implicit$8 = function (a) {
    //return (UInt32)((a.Low | a.Mid << 24) & UInt32.MaxValue);
    // return (a.$low | a.$mid << 24) & 4294967295;
    throw new ss.NotImplementedException();
};
$ss_UInt64.op_Explicit$5 = function (a) {
    return 16777216 * (16777216 * a.$high + a.$mid) + a.$low;
};
$ss_UInt64.op_Explicit$6 = function (a) {
    return 16777216 * (16777216 * a.$high + a.$mid) + a.$low;
};
$ss_UInt64.op_Explicit$4 = function (a) {
    return 16777216 * (16777216 * a.$high + a.$mid) + a.$low;
};
global.ss.UInt64 = $ss_UInt64;
ss.initClass($ss_UInt64, $asm, {
    format$1: function (format) {
        return this.format(format);
    },
    localeFormat: function (format) {
        return this.format(format);
    },
    toString: function () {
        return this.format(null);
    },
    format: function (format) {
        var ten = new $ss_UInt64(10, 0, 0);
        var a = this;
        var s = '';
        do {
            var r = $ss_UInt64.op_Modulus(a, ten);
            s = r.$low.toString() + s;
            a = $ss_UInt64.op_Division(a, ten);
        } while ($ss_UInt64.op_GreaterThan(a, $ss_UInt64.zero));
        return s;
    },
    compareTo: function (other) {
        if ($ss_UInt64.op_LessThan(this, other)) {
            return -1;
        }
        return ($ss_UInt64.op_GreaterThan(this, other) ? 1 : 0);
    },
    equalsT: function (other) {
        return this.$low === other.$low && this.$mid === other.$mid && this.$high === other.$high;
    },
    equals: function (obj) {
        if (ss.referenceEquals(null, obj)) {
            return false;
        }
        return ss.isInstanceOfType(obj, $ss_UInt64) && this.equalsT(ss.unbox(ss.cast(obj, $ss_UInt64)));
    },
    getHashCode: function () {
        var hashCode = this.$low;
        hashCode = hashCode * 397 ^ this.$mid;
        hashCode = hashCode * 397 ^ this.$high;
        return hashCode;
    }
}, null, [ss.IComparable, ss.IEquatable, ss.IFormattable]);
$ss_UInt64.__class = false;
(function () {
    $ss_UInt64.minValue = new $ss_UInt64(0, 0, 0);
    $ss_UInt64.zero = new $ss_UInt64(0, 0, 0);
    $ss_UInt64.one = new $ss_UInt64(1, 0, 0);
    $ss_UInt64.maxValue = new $ss_UInt64(16777215, 16777215, 32767);
    $ss_UInt64.$mask = -16777216;
})();