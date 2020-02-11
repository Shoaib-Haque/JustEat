function Increment() {    
    var q = document.getElementById("quantity").value;
    q++;
    document.getElementById("quantity").value = q;

    var u = document.getElementById("unitprice").value);
    var tp = document.getElementById("totalprice").value);
    tp = tp+ u;
    document.getElementById("totalprice").value = tp;

    var ta = parseFloat()(document.getElementById("totalammount").value);
    ta = ta + u;
    document.getElementById("totalammount").value = ta;

    var dr = parseFloat()(document.getElementById("discountrate").value);
    var d = parseFloat()(document.getElementById("discount").value);
    var fa = parseFloat()(document.getElementById("finalammount").value);
    var discount = (ta / 100) * dr;
    document.getElementById("discount").value = d;
    fa = ta - d;
    document.getElementById("finalammount").value = fa;
}
function d(){
    var q = document.getElementById("t").value;
    q++;
    document.getElementById("t").value = q;
}