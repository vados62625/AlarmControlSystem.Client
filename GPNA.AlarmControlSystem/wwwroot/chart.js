
window.setup = (id, config) => {
    var ctx = document.getElementById(id).getContext('2d');
    new Chart(ctx, config);
}

window.setup2 = (id, config) => {
    var ctx = document.getElementById(id).getContext('2d');
    config.options.scales.y = { min: 0, max:100 };
    new Chart(ctx, config);
}


window.setup3 = (id, config) => {
    var ctx = document.getElementById(id).getContext('2d');
    config.options.scales.y = { min: 0};
    new Chart(ctx, config);
}

window.saveComment = (id) => {
    var x = document.getElementById("snackbar");
    x.className = "show";
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 2000);
}