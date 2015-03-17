﻿////////////////////////////////////////////////////////////////////////////////
// ss.Int64
var $ss_Int64 = function (low, mid, high) {
    this.$low = 0;
    this.$mid = 0;
    this.$high = 0;
    this.$low = low & 16777215;
    this.$mid = mid & 16777215;
    this.$high = high & 65535;
};
$ss_Int64.__typeName = 'ss.Int64';
$ss_Int64.createInstance = function () {
    return ss.Int32.Zero;
};
$ss_Int64.getDefaultValue = $ss_Int64.createInstance;
$ss_Int64.parse = function (text) {
    var result = {};
    if (!$ss_Int64.tryParse(text, result)) {
        throw new ss.FormatException('Input string was not in a correct format.');
    }
    return result.$;
};
$ss_Int64.tryParse = function (text, result) {
    var radix = 10;
    result.$ = $ss_Int64.zero;
    //if (style & System.Globalization.NumberStyles.AllowHexSpecifier)
    //    radix = 16;
    var rdx = new $ss_Int64(radix, 0, 0);
    var neg = false;
    for (var i = 0; i < text.length; i++) {
        if (i === 0 && text.charCodeAt(i) === 45) {
            neg = true;
            continue;
        }
        var c = parseInt(String.fromCharCode(text.charCodeAt(i)), radix);
        if (isNaN(c)) {
            result.$ = $ss_Int64.zero;
            return false;
        }
        result.$ = $ss_Int64.op_Addition(new $ss_Int64(c, 0, 0), $ss_Int64.op_Multiply(rdx, result.$));
    }
    if (neg) {
        result.$ = $ss_Int64.op_UnaryNegation(result.$);
    }
    return true;
};
$ss_Int64.op_Addition = function (a, b) {
    var cLow = a.$low + b.$low;
    var rLow = (cLow & $ss_Int64.$mask) >> 24;
    var cMid = rLow + a.$mid + b.$mid;
    var rMid = (cMid & $ss_Int64.$mask) >> 24;
    var cHigh = rMid + a.$high + b.$high;
    return new $ss_Int64(cLow, cMid, cHigh);
};
$ss_Int64.op_Subtraction = function (a, b) {
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
    return new $ss_Int64(cLow, cMid, cHigh);
};
$ss_Int64.op_Multiply = function (a, b) {
    if (a.equalsT($ss_Int64.zero) || b.equalsT($ss_Int64.zero)) {
        return $ss_Int64.zero;
    }
    if ($ss_UInt64.op_GreaterThan($ss_UInt64.op_Explicit$6(a), $ss_UInt64.op_Explicit$6(b))) {
        return $ss_Int64.op_Multiply(b, a);
    }
    var c = $ss_Int64.zero;
    if ((a.$low & 1) === 1) {
        c = b;
    }
    var au = $ss_UInt64.op_Explicit$6(a);
    while ($ss_UInt64.op_Inequality(au, $ss_UInt64.one)) {
        au = $ss_UInt64.op_RightShift(au, 1);
        b = $ss_Int64.op_LeftShift(b, 1);
        if ((au.$low & 1) === 1) {
            c = $ss_Int64.op_Addition(c, b);
        }
    }
    return c;
};
$ss_Int64.op_Division = function (a, b) {
    debugger;
    if (b.equalsT($ss_Int64.zero)) {
        throw new ss.DivideByZeroException();
    }
    if (a.equalsT($ss_Int64.zero)) {
        return $ss_Int64.zero;
    }
    if (a.equalsT(b)) {
        return $ss_Int64.one;
    }
    var negate = a.get_$isNegative() !== b.get_$isNegative();
    var c = $ss_UInt64.op_Division($ss_UInt64.op_Explicit$6(a.get_abs()), $ss_UInt64.op_Explicit$6(b.get_abs()));
    return (negate ? $ss_UInt64.op_UnaryNegation(c) : $ss_Int64.op_Explicit$b(c));
};
$ss_Int64.op_Modulus = function (a, b) {
    if (b.equalsT($ss_Int64.zero)) {
        throw new ss.DivideByZeroException();
    }
    if (a.equalsT($ss_Int64.zero)) {
        return $ss_Int64.zero;
    }
    if (a.equalsT(b)) {
        return $ss_Int64.zero;
    }
    var negate = a.get_$isNegative();
    var c = $ss_UInt64.op_Modulus($ss_UInt64.op_Explicit$6(a.get_abs()), $ss_UInt64.op_Explicit$6(b.get_abs()));
    return (negate ? $ss_UInt64.op_UnaryNegation(c) : $ss_Int64.op_Explicit$b(c));
};
$ss_Int64.op_BitwiseAnd = function (a, b) {
    return new $ss_Int64(a.$low & b.$low, a.$mid & b.$mid, a.$high & b.$high);
};
$ss_Int64.op_BitwiseOr = function (a, b) {
    return new $ss_Int64(a.$low | b.$low, a.$mid | b.$mid, a.$high | b.$high);
};
$ss_Int64.op_ExclusiveOr = function (a, b) {
    return new $ss_Int64(a.$low ^ b.$low, a.$mid ^ b.$mid, a.$high ^ b.$high);
};
$ss_Int64.op_LeftShift = function (a, b) {
    b = b & 63;
    if (b === 0) {
        return a;
    }
    var cLow, cMid, cHigh;
    if (b <= 24) {
        cLow = a.$low << b;
        cMid = a.$low >> 24 - b | a.$mid << b;
        cHigh = a.$mid >> 24 - b | a.$high << b;
    }
    else if (b <= 48) {
        cLow = 0;
        cMid = a.$low << b - 24;
        cHigh = a.$low >> 48 - b | a.$mid << b - 24;
    }
    else {
        cLow = 0;
        cMid = 0;
        cHigh = a.$low << b - 48;
    }
    return new $ss_Int64(cLow, cMid, cHigh);
};
$ss_Int64.op_RightShift = function (a, b) {
    // Int64 (signed) uses arithmetic shift, UIn64 (unsigned) uses logical shift
    b = b & 63;
    if (b === 0) {
        return a;
    }
    var aHigh = (a.get_$isNegative() ? (-65536 | a.$high) : a.$high);
    var cLow, cMid, cHigh;
    if (b <= 24) {
        cLow = a.$mid << 24 - b | a.$low >> b;
        cMid = aHigh << 24 - b | a.$mid >> b;
        cHigh = aHigh >> b;
    }
    else if (b <= 48) {
        cLow = aHigh << 48 - b | a.$mid >> b - 24;
        cMid = aHigh >> b - 24;
        cHigh = (a.get_$isNegative() ? 65535 : 0);
    }
    else {
        cLow = aHigh >> b - 48;
        cMid = (a.get_$isNegative() ? 16777215 : 0);
        cHigh = (a.get_$isNegative() ? 65535 : 0);
    }
    return new $ss_Int64(cLow, cMid, cHigh);
};
$ss_Int64.op_Equality = function (a, b) {
    return a.equalsT(b);
};
$ss_Int64.op_Inequality = function (a, b) {
    return !a.equalsT(b);
};
$ss_Int64.op_LessThanOrEqual = function (a, b) {
    return ((a.get_$isNegative() === b.get_$isNegative()) ? $ss_UInt64.op_LessThanOrEqual($ss_UInt64.op_Explicit$6(a), $ss_UInt64.op_Explicit$6(b)) : a.get_$isNegative());
};
$ss_Int64.op_GreaterThanOrEqual = function (a, b) {
    return ((a.get_$isNegative() === b.get_$isNegative()) ? $ss_UInt64.op_GreaterThanOrEqual($ss_UInt64.op_Explicit$6(a), $ss_UInt64.op_Explicit$6(b)) : b.get_$isNegative());
};
$ss_Int64.op_LessThan = function (a, b) {
    return ((a.get_$isNegative() === b.get_$isNegative()) ? $ss_UInt64.op_LessThan($ss_UInt64.op_Explicit$6(a), $ss_UInt64.op_Explicit$6(b)) : a.get_$isNegative());
};
$ss_Int64.op_GreaterThan = function (a, b) {
    return ((a.get_$isNegative() === b.get_$isNegative()) ? $ss_UInt64.op_GreaterThan($ss_UInt64.op_Explicit$6(a), $ss_UInt64.op_Explicit$6(b)) : b.get_$isNegative());
};
$ss_Int64.op_UnaryNegation = function (a) {
    return $ss_Int64.op_Addition($ss_Int64.op_OnesComplement(a), $ss_Int64.one);
};
$ss_Int64.op_OnesComplement = function (a) {
    return new $ss_Int64(~a.$low, ~a.$mid, ~a.$high);
};
$ss_Int64.op_Increment = function (a) {
    var cLow = a.$low + 1;
    var rLow = (cLow & $ss_Int64.$mask) >> 24;
    var cMid = rLow + a.$mid;
    var rMid = (cMid & $ss_Int64.$mask) >> 24;
    var cHigh = rMid + a.$high;
    return new $ss_Int64(cLow, cMid, cHigh);
};
$ss_Int64.op_Decrement = function (a) {
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
    return new $ss_Int64(cLow, cMid, cHigh);
};
$ss_Int64.op_Explicit$b = function (a) {
    return new $ss_Int64(a.$low, a.$mid, a.$high);
};
$ss_Int64.op_Implicit = function (a) {
    return new $ss_Int64(a, 0, 0);
};
$ss_Int64.op_Implicit$3 = function (a) {
    return new $ss_Int64(a, ((a < 0) ? 16777215 : 0), ((a < 0) ? 65535 : 0));
};
$ss_Int64.op_Implicit$4 = function (a) {
    return new $ss_Int64(a, 0, 0);
};
$ss_Int64.op_Implicit$1 = function (a) {
    return new $ss_Int64(a, ((a < 0) ? 16777215 : 0), ((a < 0) ? 65535 : 0));
};
$ss_Int64.op_Implicit$5 = function (a) {
    return new $ss_Int64(a, a >>> 24, 0);
};
$ss_Int64.op_Implicit$2 = function (a) {
    return new $ss_Int64(a, a >> 24, ((a < 0) ? 65535 : 0));
};
$ss_Int64.op_Explicit$1 = function (a) {
    var r = $ss_Int64.op_Explicit$b($ss_UInt64.op_Explicit$1(Math.abs(a)));
    return ((a < 0) ? $ss_Int64.op_UnaryNegation(r) : r);
};
$ss_Int64.op_Explicit$2 = function (a) {
    var r = $ss_Int64.op_Explicit$b($ss_UInt64.op_Explicit$5(Math.abs(a)));
    return ((a < 0) ? $ss_Int64.op_UnaryNegation(r) : r);
};
$ss_Int64.op_Explicit = function (a) {
    var r = $ss_Int64.op_Explicit$b($ss_UInt64.op_Explicit(Math.abs(a)));
    return ((a < 0) ? $ss_Int64.op_UnaryNegation(r) : r);
};
$ss_Int64.op_Explicit$3 = function (a) {
    return a.$low & 255;
};
$ss_Int64.op_Explicit$7 = function (a) {
    return a.$low & 255;
};
$ss_Int64.op_Explicit$9 = function (a) {
    return a.$low & 65535;
};
$ss_Int64.op_Explicit$5 = function (a) {
    return a.$low & 65535;
};
$ss_Int64.op_Explicit$a = function (a) {
    //return (UInt32)((a.Low | a.Mid << 24) & UInt32.MaxValue);
    // return (a.$low | a.$mid << 24) & 4294967295;
    throw new ss.NotImplementedException();
};
$ss_Int64.op_Explicit$6 = function (a) {
    //return (Int32)((a.Low | a.Mid << 24) & UInt32.MaxValue);
    // return (a.$low | a.$mid << 24) & 4294967295;
    throw new ss.NotImplementedException();
};
$ss_Int64.op_Explicit$4 = function (a) {
    return (a.get_$isNegative() ? -$ss_UInt64.op_Explicit$8($ss_UInt64.op_Explicit$6($ss_Int64.op_UnaryNegation(a))) : $ss_UInt64.op_Explicit$8($ss_UInt64.op_Explicit$6(a)));
};
$ss_Int64.op_Explicit$8 = function (a) {
    return (a.get_$isNegative() ? -$ss_UInt64.op_Explicit$c($ss_UInt64.op_Explicit$6($ss_Int64.op_UnaryNegation(a))) : $ss_UInt64.op_Explicit$c($ss_UInt64.op_Explicit$6(a)));
};
$ss_Int64.op_Implicit$6 = function (a) {
    return (a.get_$isNegative() ? -$ss_UInt64.op_Implicit$3($ss_UInt64.op_Explicit$6($ss_Int64.op_UnaryNegation(a))) : $ss_UInt64.op_Implicit$3($ss_UInt64.op_Explicit$6(a)));
};
global.ss.Int64 = $ss_Int64;
ss.initClass($ss_Int64, $asm, {
    get_$isNegative: function () {
        return (this.$high & 32768) !== 0;
    },
    get_abs: function () {
        return (this.get_$isNegative() ? $ss_Int64.op_UnaryNegation(this) : this);
    },
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
        return (this.get_$isNegative() ? ('-' + $ss_UInt64.op_Explicit$6($ss_Int64.op_UnaryNegation(this)).format(format)) : $ss_UInt64.op_Explicit$6(this).format(format));
    },
    compareTo: function (other) {
        if ($ss_Int64.op_LessThan(this, other)) {
            return -1;
        }
        if ($ss_Int64.op_GreaterThan(this, other)) {
            return 1;
        }
        return 0;
    },
    equalsT: function (other) {
        return this.$low === other.$low && this.$mid === other.$mid && this.$high === other.$high;
    },
    equals: function (obj) {
        if (ss.referenceEquals(null, obj)) {
            return false;
        }
        return ss.isInstanceOfType(obj, $ss_Int64) && this.equalsT(ss.unbox(ss.cast(obj, $ss_Int64)));
    },
    getHashCode: function () {
        var hashCode = this.$low;
        hashCode = hashCode * 397 ^ this.$mid;
        hashCode = hashCode * 397 ^ this.$high;
        return hashCode;
    }
}, null, [ss.IComparable, ss.IEquatable, ss.IFormattable]);
$ss_Int64.__class = false;
(function () {
    $ss_Int64.minValue = new $ss_Int64(0, 0, 32768);
    $ss_Int64.zero = new $ss_Int64(0, 0, 0);
    $ss_Int64.one = new $ss_Int64(1, 0, 0);
    $ss_Int64.maxValue = new $ss_Int64(16777215, 16777215, 32767);
    $ss_Int64.$mask = -16777216;
})();