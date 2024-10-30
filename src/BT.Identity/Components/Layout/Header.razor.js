
export function toggleSidebar(currentValue) {
    var element = document.getElementById("sidebar");
    if (currentValue == 0) {        
        element.classList.add("-translate-x-full");
        element.classList.remove("translate-x-0");
        console.log("Sidebar closed");
    } else {
        //  open
        element.classList.add("translate-x-0");
        element.classList.remove("-translate-x-full");
        console.log("Sidebar open");
    }
}