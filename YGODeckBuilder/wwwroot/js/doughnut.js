document.addEventListener("DOMContentLoaded", function () {
    var doughnutChart = document.getElementById("doughnutChart");
    var allDeckTypes = [
        // ... (your deck types)
        "Effect Monster",
        "Flip Effect Monster",
        "Flip Tuner Effect Monster",
        "Gemini Monster",
        "Normal Monster",
        "Normal Tuner Monster",
        "Spirit Monster",
        "Toon Monster", 
        "Tuner Monster",
        "Union Effect Monster",
        "Pendulum Effect Monster",
        "Pendulum Effect Ritual Monster",
        "Pendulum Flip Effect Monster",
        "Pendulum Normal Monster",
        "Pendulum Tuner Effect Monster",
        "Ritual Effect Monster",
        "Ritual Monster",
        "Spell Card",
        "Trap Card",
        "Fusion Monster",
        "Link Monster",
        "Pendulum Effect Fusion Monster",
        "Synchro Monster",
        "Synchro Pendulum Effect Monster",
        "Synchro Tuner Monster",
        "XYZ Monster",
        "XYZ Pendulum Effect Monster"
    ];
    var typeColors = {
        "Effect Monster": "#d99100", 
        "Flip Effect Monster": "#996a0b", 
        "Flip Tuner Effect Monster": "#946506", 
        "Gemini Monster": "#d49924", 
        "Normal Monster": "#f2c729", 
        "Normal Tuner Monster": "#f5ca31", 
        "Pendulum Effect Monster": "#e09909", 
        "Pendulum Effect Ritual Monster": "#e09909", 
        "Pendulum Flip Effect Monster": "#e09909", 
        "Pendulum Normal Monster": "#e09909", 
        "Pendulum Tuner Effect Monster": "#e09909", 
        "Ritual Effect Monster": "#0b699c", 
        "Ritual Monster": "#389acf", 
        "Spell Card": "#0a8c7d",
        "Spirit Monster": "#f5ca31", 
        "Toon Monster": "#f5ca31", 
        "Trap Card": "#b3007a", 
        "Tuner Monster": "#f5ca31", 
        "Union Effect Monster": "#FFFFFF", 
        "Fusion Monster": "#FFFFFF", 
        "Link Monster": "#084ebf", 
        "Pendulum Effect Fusion Monster": "#FFFFFF", 
        "Synchro Monster": "#cfcfcf", 
        "Synchro Pendulum Effect Monster": "#cfcfcf", 
        "Synchro Tuner Monster": "#cfcfcf", 
        "XYZ Monster": "#2b2b2b", 
        "XYZ Pendulum Effect Monster": "#4a4a4a" 
    };

    // Access the deck data from the exposed JavaScript variables
    var mainDeckData = window.mainDeckData;
    var extraDeckData = window.extraDeckData;
    
    // Continue with your code to count and draw the doughnut chart
    var deckCounts = allDeckTypes.map(function (type) {
        console.log("Type:", type);
        var mainDeckCount = mainDeckData.filter(c => c.type === type).length;
        console.log("Main Deck Count for", type, ":", mainDeckCount);
        var extraDeckCount = extraDeckData.filter(c => c.type === type).length;
        console.log("Extra Deck Count for", type, ":", extraDeckCount);
        return {
            title: type,
            value: mainDeckCount + extraDeckCount,
            color: typeColors[type] || getRandomColor()
        };
    });

    drawDoughnutChart(doughnutChart, deckCounts);
});
function drawDoughnutChart(chart, data) {
    var W = chart.offsetWidth,
        H = chart.offsetHeight,
        centerX = W / 2,
        centerY = H / 2,
        cos = Math.cos,
        sin = Math.sin,
        PI = Math.PI;

    var settings = {
        segmentShowStroke: true,
        segmentStrokeColor: "#0C1013",
        segmentStrokeWidth: 1,
        baseColor: "rgba(0,0,0,0.5)",
        baseOffset: 4,
        edgeOffset: 10,
        percentageInnerCutout: 75,
        animation: true,
        animationSteps: 100,
        animationEasing: "easeInOutExpo",
        animateRotate: true,
        tipOffsetX: -8,
        tipOffsetY: -45,
        tipClass: "doughnutTip",
        summaryClass: "doughnutSummary",
        summaryTitle: "Total Cards:",
        summaryTitleClass: "doughnutSummaryTitle",
        summaryNumberClass: "doughnutSummaryNumber",
        beforeDraw: function () { },
        afterDrawed: function () { },
        onPathEnter: function (e, data) { },
        onPathLeave: function (e, data) { },
    };

    var animationOptions = {
        linear: function (t) {
            return t;
        },
        easeInOutExpo: function (t) {
            var v = t < 0.5 ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t;
            return v > 1 ? 1 : v;
        },
    };

    function getHollowCirclePath(doughnutRadius, cutoutRadius) {
        var startRadius = -1.570, // -Math.PI/2
            segmentAngle = 6.2831, // 1 * ((99.9999/100) * (PI*2)),
            endRadius = 4.7131, // startRadius + segmentAngle
            startX = centerX + cos(startRadius) * doughnutRadius,
            startY = centerY + sin(startRadius) * doughnutRadius,
            endX2 = centerX + cos(startRadius) * cutoutRadius,
            endY2 = centerY + sin(startRadius) * cutoutRadius,
            endX = centerX + cos(endRadius) * doughnutRadius,
            endY = centerY + sin(endRadius) * doughnutRadius,
            startX2 = centerX + cos(endRadius) * cutoutRadius,
            startY2 = centerY + sin(endRadius) * cutoutRadius;

        var cmd = [
            "M",
            startX,
            startY,
            "A",
            doughnutRadius,
            doughnutRadius,
            0,
            1,
            1,
            endX,
            endY,
            "Z",
            "M",
            startX2,
            startY2,
            "A",
            cutoutRadius,
            cutoutRadius,
            0,
            1,
            0,
            endX2,
            endY2,
            "Z",
        ];
        cmd = cmd.join(" ");
        return cmd;
    }

    function pathMouseEnter(e) {
        var order = parseInt(this.getAttribute("data-order"));
        $tip.innerText = data[order].title + ": " + data[order].value;
        $tip.style.display = "block";
        settings.onPathEnter.apply(this, [e, data]);
    }

    function pathMouseLeave(e) {
        $tip.style.display = "none";
        settings.onPathLeave.apply(this, [e, data]);
    }

    function pathMouseMove(e) {
        $tip.style.top = e.pageY + settings.tipOffsetY + "px";
        $tip.style.left = e.pageX - $tip.offsetWidth / 2 + settings.tipOffsetX + "px";
    }

    function drawPieSegments(animationDecimal) {
        var startRadius = -PI / 2,
            rotateAnimation = 1;

        if (settings.animation && settings.animateRotate) {
            rotateAnimation = animationDecimal;
        }

        drawDoughnutText(animationDecimal, segmentTotal);

        $pathGroup.style.opacity = animationDecimal;

        if (data.length === 1 && 4.7122 < rotateAnimation * ((data[0].value / segmentTotal) * (PI * 2)) + startRadius) {
            $paths[0].setAttribute("d", getHollowCirclePath(doughnutRadius, cutoutRadius));
            return;
        }

        for (var i = 0, len = data.length; i < len; i++) {
            var segmentAngle = rotateAnimation * ((data[i].value / segmentTotal) * (PI * 2)),
                endRadius = startRadius + segmentAngle,
                largeArc = ((endRadius - startRadius) % (PI * 2)) > PI ? 1 : 0,
                startX = centerX + cos(startRadius) * doughnutRadius,
                startY = centerY + sin(startRadius) * doughnutRadius,
                endX2 = centerX + cos(startRadius) * cutoutRadius,
                endY2 = centerY + sin(startRadius) * cutoutRadius,
                endX = centerX + cos(endRadius) * doughnutRadius,
                endY = centerY + sin(endRadius) * doughnutRadius,
                startX2 = centerX + cos(endRadius) * cutoutRadius,
                startY2 = centerY + sin(endRadius) * cutoutRadius;

            var cmd = [
                "M",
                startX,
                startY,
                "A",
                doughnutRadius,
                doughnutRadius,
                0,
                largeArc,
                1,
                endX,
                endY,
                "L",
                startX2,
                startY2,
                "A",
                cutoutRadius,
                cutoutRadius,
                0,
                largeArc,
                0,
                endX2,
                endY2,
                "Z",
            ];

            $paths[i].setAttribute("d", cmd.join(" "));
            startRadius += segmentAngle;
        }
    }

    function drawDoughnutText(animationDecimal, segmentTotal) {
        $summaryNumber.style.opacity = animationDecimal;
        $summaryNumber.textContent = (segmentTotal * animationDecimal).toFixed(1);
    }

    function animateFrame(cnt, drawData) {
        var easeAdjustedAnimationPercent = (settings.animation) ? CapValue(easingFunction(cnt), null, 0) : 1;
        drawData(easeAdjustedAnimationPercent);
    }

    function animationLoop(drawData) {
        var animFrameAmount = (settings.animation) ? 1 / CapValue(settings.animationSteps, Number.MAX_VALUE, 1) : 1;
        var cnt = (settings.animation) ? 0 : 1;
        function animate() {
            cnt += animFrameAmount;
            animateFrame(cnt, drawData);
            if (cnt <= 1) {
                requestAnimationFrame(animate);
            } else {
                settings.afterDrawed.call(animationLoop($this));
            }
        }
        animate();
    }

    function Max(arr) {
        return Math.max.apply(null, arr);
    }

    function Min(arr) {
        return Math.min.apply(null, arr);
    }

    function isNumber(n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    }

    function CapValue(valueToCap, maxValue, minValue) {
        if (isNumber(maxValue) && valueToCap > maxValue) return maxValue;
        if (isNumber(minValue) && valueToCap < minValue) return minValue;
        return valueToCap;
    }

    var $svg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
    $svg.setAttribute("width", W);
    $svg.setAttribute("height", H);
    $svg.setAttribute("viewBox", "0 0 " + W + " " + H);
    $svg.setAttribute("xmlns", "http://www.w3.org/2000/svg");
    $svg.setAttribute("xmlns:xlink", "http://www.w3.org/1999/xlink");

    var $paths = [];
    var easingFunction = animationOptions[settings.animationEasing];
    var doughnutRadius = Math.min(H / 2, W / 2) - settings.edgeOffset;
    var cutoutRadius = doughnutRadius * (settings.percentageInnerCutout / 100);
    var segmentTotal = 0;

    // Draw base doughnut
    var baseDoughnutRadius = doughnutRadius + settings.baseOffset;
    var baseCutoutRadius = cutoutRadius - settings.baseOffset;

    var $baseDoughnut = document.createElementNS("http://www.w3.org/2000/svg", "path");
    $baseDoughnut.setAttribute("d", getHollowCirclePath(baseDoughnutRadius, baseCutoutRadius));
    $baseDoughnut.setAttribute("fill", settings.baseColor);
    $svg.appendChild($baseDoughnut);

    // Set up pie segments wrapper
    var $pathGroup = document.createElementNS("http://www.w3.org/2000/svg", "g");
    $pathGroup.setAttribute("opacity", "0");
    $svg.appendChild($pathGroup);

    // Set up tooltip
    var $tip = document.createElement("div");
    $tip.className = settings.tipClass;
    $tip.style.display = "none";
    document.body.appendChild($tip);

    // Set up center text area
    var summarySize = (cutoutRadius - (doughnutRadius - cutoutRadius)) * 2;
    var $summary = document.createElement("div");
    $summary.className = settings.summaryClass;
    $summary.style.width = summarySize + "px";
    $summary.style.height = summarySize + "px";
    $summary.style.marginLeft = -(summarySize / 2) + "px";
    $summary.style.marginTop = -(summarySize / 2) + "px";

    var $summaryTitle = document.createElement("p");
    $summaryTitle.className = settings.summaryTitleClass;
    $summaryTitle.textContent = settings.summaryTitle;
    $summary.appendChild($summaryTitle);

    var $summaryNumber = document.createElement("p");
    $summaryNumber.className = settings.summaryNumberClass;
    $summaryNumber.style.opacity = "0";
    $summary.appendChild($summaryNumber);

    chart.appendChild($svg);
    chart.appendChild($summary);

    for (var i = 0, len = data.length; i < len; i++) {
        segmentTotal += data[i].value;
        var $path = document.createElementNS("http://www.w3.org/2000/svg", "path");
        $path.setAttribute("stroke-width", settings.segmentStrokeWidth);
        $path.setAttribute("stroke", settings.segmentStrokeColor);
        $path.setAttribute("fill", data[i].color);
        $path.setAttribute("data-order", i);
        $pathGroup.appendChild($path);

        $path.addEventListener("mouseenter", pathMouseEnter);
        $path.addEventListener("mouseleave", pathMouseLeave);
        $path.addEventListener("mousemove", pathMouseMove);

        $paths.push($path);
    }

    animationLoop(drawPieSegments);
}
function getRandomColor() {
    return "#" + ((1 << 24) * Math.random() | 0).toString(16);
}
