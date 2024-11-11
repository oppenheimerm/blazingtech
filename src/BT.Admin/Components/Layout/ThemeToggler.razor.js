
export function toggleDarkMode() {
    var element = document.getElementById("mainDiv");
    element.classList.toggle("dark");
}

export function initTheme(val) {
    if (val == 1) {
        var element = document.getElementById("mainDiv");
        element.classList.add("dark");
    } 
}


export function setTheme(val) {
    if (val == 1) {
        var element = document.getElementById("mainDiv");
        element.classList.add("dark");
    }
}