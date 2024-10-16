const patternreg = /pattern.draw\("(\w+)"\s*,\s*"(.+)"\)/i
const datereg = /\/Date\((\d+)\)\//i;
function _isObject(value) {
    return value !== null && Object.prototype.toString.call(value) === "[object Object]";
}

function _normalizeChart(chart) {
    chart.data.datasets.forEach(ds => {
        _normalizeColor(ds, "backgroundColor");
        _normalizeColor(ds, "borderColor");
        _normalizeColor(ds, "hoverBackgroundColor");
        _normalizeColor(ds, "hoverBorderColor");
        _normalizeColor(ds, "pointBackgroundColor");
        _normalizeColor(ds, "pointBorderColor");
        _normalizeColor(ds, "pointHoverBackgroundColor");
        _normalizeColor(ds, "pointHoverBorderColor");

        if (Array.isArray(ds.data)) {
            ds.data.forEach(d => {
                _normalizeDate(d);
            });
        } else {
            _normalizeDate(ds.data);
        }

        if (chart.useDataLabels) {
            _normalizeDataLabels(ds, chart.options.plugins.datalabels);
        }
    });

    if (chart.options != null) {
        if (chart.options.plugins != null) {
            if (chart.options.plugins.title != null) {
                _normalizeColor(chart.options.plugins.title, "color");
            }
            if (chart.options.plugins.subtitle != null) {
                _normalizeColor(chart.options.plugins.subtitle, "color");
            }
            if (chart.options.plugins.legend != null) {
                if (chart.options.plugins.legend.labels != null) {
                    _normalizeColor(chart.options.plugins.legend.labels, "color");
                }
                if (chart.options.plugins.legend.title != null) {
                    _normalizeColor(chart.options.plugins.legend.title, "color");
                }
            }
            if (chart.options.plugins.tooltip != null) {
                _normalizeColor(chart.options.plugins.tooltip, "backgroundColor");
                _normalizeColor(chart.options.plugins.tooltip, "titleColor");
                _normalizeColor(chart.options.plugins.tooltip, "bodyColor");
                _normalizeColor(chart.options.plugins.tooltip, "footerColor");
                _normalizeColor(chart.options.plugins.tooltip, "multikeyBackground");
                _normalizeColor(chart.options.plugins.tooltip, "borderColor");
            }
            if (chart.options.plugins.datalabels != null) {
                _normalizeColor(chart.options.plugins.datalabels, "color");
            }
        }
        if (chart.options.scales != null) {
            Object.keys(chart.options.scales).forEach(key => {
                const scale = chart.options.scales[key];
                if (scale.grid != null) {
                    _normalizeColor(scale.grid, "borderColor");
                    _normalizeColor(scale.grid, "color");
                    _normalizeColor(scale.grid, "tickColor");
                }
                if (scale.ticks != null) {
                    _normalizeColor(scale.ticks, "backdropColor");
                    _normalizeColor(scale.ticks, "color");
                    _normalizeColor(scale.ticks, "textStrokeColor");

                    if (scale.ticks.format != null) {
                        _normalizeFormat(scale.ticks, scale.ticks.format);
                    }
                }
            });
        }

        if (chart.options.responsive != null
            && chart.options.responsive == true
            && chart.options.aspectRatio == null
            && chart.options.maintainAspectRatio == null
            && chart.width == null
            && chart.height == null) {
            if (window.innerWidth < 768) {
                chart.options.aspectRatio = 1;
            }
        }

    }

    if (chart.useDataLabels != null && chart.useDataLabels == true) {
        chart.plugins = [];
        chart.plugins.push(ChartDataLabels);
    }

    _normalizeFunction(chart);
}

function _normalizeDataLabels(dataset, datalabels) {
    if (datalabels.format != null) {
        dataset.datalabels = {
            format: datalabels.format,
            formatter: function (value, context) {
                let val = null;
                if (context.dataset.parsing != null) {
                    val = value[context.dataset.parsing.yAxisKey];
                } else {
                    val = value;
                }
                if (context.dataset.datalabels.format != null) {
                    return val.toLocaleString("es-CL", context.dataset.datalabels.format);
                }

                return val;
            }
        };
    }
}

function _normalizeFormat(tick, format) {
    tick.callback = function (value, index, ticks) {
        return value.toLocaleString("es-CL", format);
    };
}

function _normalizeDate(data) {
    if (data) {
        Object.keys(data).forEach(key => {
            if (typeof data[key] === "string" && data[key].startsWith("/Date")) {
                const value = data[key];
                const match = value.match(datereg);
                if (match) {
                    data[key] = new Date(parseInt(match[1])).toISOString();
                }
            }
        });
    }
}

function _normalizeColor(o, propertyName) {
    if (o[propertyName] != null) {
        const value = o[propertyName];

        if (Array.isArray(value)) {
            const newColors = new Array();
            value.forEach(v => {
                const result = _extractPatternColor(v);
                if (result.succeeded) {
                    newColors.push(pattern.draw(result.source, result.color));
                } else {
                    newColors.push(v);
                }
            });
            o[propertyName] = newColors;
        } else {
            const result = _extractPatternColor(value);
            if (result.succeeded) {
                o[propertyName] = pattern.draw(result.source, result.color);
            }
        }
    }
}

function _extractPatternColor(value) {
    const match = value.match(patternreg);

    if (match) {
        const source = match[1];
        const color = match[2];

        return {
            succeeded: true,
            source: source,
            color: color
        }
    }

    return {
        succeeded: false
    };
}

function _normalizeFunction(d) {
    if (_isObject(d)) {
        Object.keys(d).forEach(key => {
            let val = d[key];

            if (Array.isArray(val)) {
                val.forEach(item => _normalizeFunction(item));
            } else if (_isObject(val)) {
                _normalizeFunction(val);
            } else if (typeof val == "string") {
                let func = _findFunction(val);
                if (func != null) {
                    d[key] = func;
                }
            }
        });
    }
}

function _findFunction(value) {
    const script = value.trim();
    if (script.startsWith("function")) {
        let parOpenPos = script.indexOf("(");
        let parClosePos = script.indexOf(")", parOpenPos);
        let braOpenPos = script.indexOf("{", parClosePos);
        let braClosePos = script.lastIndexOf("}");

        if (parOpenPos > -1 && parClosePos > -1 && braOpenPos > -1 && braClosePos > -1) {
            let parameterStr = script.substring(parOpenPos + 1, parClosePos).trim();
            let parameters = parameterStr.split(",");
            parameters = parameters.map(p => p.trim());

            let body = script.substring(braOpenPos + 1, braClosePos).trim();

            return new Function(parameters, body);
        }
    } else if (script.startsWith("(")) {
        let parClosePos = script.indexOf(")");
        let arrowPos = script.indexOf("=>", parClosePos);
        let braOpenPos = script.indexOf("{", arrowPos);
        let braClosePos = script.lastIndexOf("}");

        if (parClosePos > -1 && arrowPos > -1) {
            let parameterStr = script.substring(1, parClosePos).trim();
            let parameters = parameterStr.split(",");
            parameters = parameters.map(p => p.trim());

            let body = null;
            if (braOpenPos > -1 && braClosePos > -1) {
                body = script.substring(braOpenPos + 1, braClosePos);
            } else {
                body = script.substring(arrowPos + 2).trim();
            }

            if (body) {
                return new Function(parameters, body);
            }
        }
    }
    return null;
}

if (!window.ozyparkadmin) {
    window.ozyparkadmin = {};
}

if (!window.ozyparkadmin.chartjs) {
    window.ozyparkadmin.chartjs = {};
}

window.ozyparkadmin.chartjs = {
    initialize: (elementId, type, data, options, plugins) => {
        let chart = window.ozyparkadmin.chartjs.get(elementId);
        if (chart) {
            window.ozyparkadmin.chartjs.update(elementId, data, options);
            return;
        }
        window.ozyparkadmin.chartjs.create(elementId, type, data, options, plugins);
    },
    get: (elementId) => {
        let chart;
        Chart.helpers.each(Chart.instances, function (instance) {
            if (instance.canvas.id === elementId) {
                chart = instance;
            }
        });

        return chart;
    },
    create: (elementId, type, data, options) => {
        let chartEl = document.getElementById(elementId);

        const config = {
            type: type,
            data: data,
            options: options,
        };

        _normalizeChart(config);

        const chart = new Chart(
            chartEl,
            config
        );
    },
    update: (elementId, data, options) => {
        let chart = window.ozyparkadmin.chartjs.get(elementId);
        if (chart) {
            if (chart.config.plugins && chart.config.plugins.findIndex(x => x.id === "datalabels") > -1) {
                options.plugins.datalabels.backgroundColor = function (context) {
                    return context.dataset.backgroundColor;
                };
            }

            const config = {
                data: data,
                options: options,
            };

            _normalizeChart(config);

            chart.data = config.data;
            chart.options = config.options;
            chart.update();
        }
    },
    resize: (elementId, width, height) => {
        let chart = window.ozyparkadmin.chartjs.get(elementId);
        if (chart) {
            chart.canvas.style.height = height;
            chart.canvas.style.width = width;
        }
    },
    destroy: (elementId) => {
        let chart = window.ozyparkadmin.chartjs.get(elementId);
        if (chart) {
            chart.destroy();
        }
    }
}