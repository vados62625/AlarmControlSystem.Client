Chart.register({
    id: 'doughnut-centertext',
    beforeDraw: function (chart) {
        if (chart.config.options.elements && chart.config.options.elements.center) {
            // Get ctx from string
            var ctx = chart.ctx;

            // Get options from the center object in options
            var centerConfig = chart.config.options.elements.center;
            var fontStyle = centerConfig.fontStyle || 'Arial';
            var txt = centerConfig.text;
            var color = centerConfig.color || '#000';
            var maxFontSize = centerConfig.maxFontSize || 75;
            var sidePadding = centerConfig.sidePadding || 20;
            var sidePaddingCalculated = (sidePadding / 100) * (chart._metasets[0].data[0].innerRadius * 2)
            // Start with a base font of 30px
            ctx.font = "30px " + fontStyle;

            // Get the width of the string and also the width of the element minus 10 to give it 5px side padding
            var stringWidth = ctx.measureText(txt).width;
            var elementWidth = (chart._metasets[0].data[0].innerRadius * 2) - sidePaddingCalculated;

            // Find out how much the font can grow in width.
            var widthRatio = elementWidth / stringWidth;
            var newFontSize = Math.floor(30 * widthRatio);
            var elementHeight = (chart._metasets[0].data[0].innerRadius * 2);

            // Pick a new font size so it will not be larger than the height of label.
            var fontSizeToUse = Math.min.apply(null, [newFontSize, elementHeight, maxFontSize]);
            var minFontSize = centerConfig.minFontSize;
            var lineHeight = centerConfig.lineHeight || 25;
            var wrapText = false;

            if (minFontSize === undefined) {
                minFontSize = 20;
            }

            if (minFontSize && fontSizeToUse < minFontSize) {
                fontSizeToUse = minFontSize;
                wrapText = true;
            }

            // Set font settings to draw it correctly.
            ctx.textAlign = 'center';
            ctx.textBaseline = 'middle';
            var centerX = ((chart.chartArea.left + chart.chartArea.right) / 2);
            var centerY = ((chart.chartArea.top + chart.chartArea.bottom) / 2);
            ctx.font = fontSizeToUse + "px " + fontStyle;
            ctx.fillStyle = color;

            if (!wrapText) {
                ctx.fillText(txt, centerX, centerY);
                return;
            }

            var words = txt.split(' ');
            var line = '';
            var lines = [];

            // Break words up into multiple lines if necessary
            for (var n = 0; n < words.length; n++) {
                var testLine = line + words[n] + ' ';
                var metrics = ctx.measureText(testLine);
                var testWidth = metrics.width;
                if (testWidth > elementWidth && n > 0) {
                    lines.push(line);
                    line = words[n] + ' ';
                } else {
                    line = testLine;
                }
            }

            // Move the center up depending on line height and number of lines
            centerY -= (lines.length / 2) * lineHeight;

            for (var n = 0; n < lines.length; n++) {
                ctx.fillText(lines[n], centerX, centerY);
                centerY += lineHeight;
            }
            //Draw text in center
            ctx.fillText(line, centerX, centerY);
        }
    }
});

Chart.register({
    id: 'emptyDoughnut',
    afterDraw(chart, args, options) {
        const {datasets} = chart.data;
        const {color, width, radiusDecrease} = options;
        let hasData = false;

        for (let i = 0; i < datasets.length; i += 1) {
            const dataset = datasets[i];
            // hasData |= dataset.data.length > 0;
            for (let j = 0; j < dataset.data.length; j += 1) {
                if (dataset.data[j] > 0) {
                    hasData = true;
                    break;
                }
            }
        }

        if (!hasData) {
            const {chartArea: {left, top, right, bottom}, ctx} = chart;
            const centerX = (left + right) / 2;
            const centerY = (top + bottom) / 2;
            const r = Math.min(right - left, bottom - top) / 2;

            ctx.beginPath();
            ctx.lineWidth = width || 2;
            ctx.strokeStyle = color || 'rgba(255, 128, 0, 0.5)';
            ctx.arc(centerX, centerY, (r - radiusDecrease || 0), 0, 2 * Math.PI);
            ctx.stroke();
        }
    }
});

window.setup = (id, config) => {
    let chartStatus = Chart.getChart(id); // <canvas> id
    if (chartStatus != undefined) {
        chartStatus.destroy();
    }
    let el = document.getElementById(id)
    if (el) {
        let ctx = el.getContext('2d');
        new Chart(ctx, config);
    }
}

window.setup2 = (id, config) => {
    let el = document.getElementById(id)
    if (el) {
        let ctx = el.getContext('2d');
        config.options.scales.y = {min: 0, max: 100};
        new Chart(ctx, config);
    }
}


window.setup3 = (id, config) => {
    let chartStatus = Chart.getChart(id); // <canvas> id
    if (chartStatus != undefined) {
        chartStatus.destroy();
    }
    let el = document.getElementById(id)
    if (el) {
        let ctx = el.getContext('2d');
        config.options.scales.y = {min: 0};
        new Chart(ctx, config);
    }

}

window.setupBarChart = (id, config) => {
    let chartStatus = Chart.getChart(id); // <canvas> id
    if (chartStatus != undefined) {
        chartStatus.destroy();
    }
    let el = document.getElementById(id)
    if (el) {
        let ctx = el.getContext('2d');
        config.options.scales.y = {min: 0};
        config.options.plugins.tooltip = {
            callbacks: {
                label: function (tooltipItem, data) {
                    var corporation = data.datasets[tooltipItem.datasetIndex].label;
                    var valor = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];

                    // Loop through all datasets to get the actual total of the index
                    var total = 0;
                    for (var i = 0; i < data.datasets.length; i++)
                        total += data.datasets[i].data[tooltipItem.index];

                    // If it is not the last dataset, you display it as you usually do
                    if (tooltipItem.datasetIndex != data.datasets.length - 1) {
                        return corporation + " : $" + valor.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
                    } else { // .. else, you display the dataset and the total, using an array
                        return [corporation + " : $" + valor.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'), "Total : $" + total];
                    }
                }
            }
        };
        
        new Chart(ctx, config);
    }

}