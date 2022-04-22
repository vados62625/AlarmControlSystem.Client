

window.setup = (id, config) => {
    var ctx = document.getElementById(id).getContext('2d');    
    new Chart(ctx, config);
}

window.setup2 = (id, config) => {
    var ctx = document.getElementById(id).getContext('2d');
    config.options.scales.y = { min: 0, max:100 };
    new Chart(ctx, config);
}