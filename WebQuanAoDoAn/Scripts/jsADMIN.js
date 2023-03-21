document.addEventListener("DOMContentLoaded", function () {
    var calendarMiniEl = document.getElementById("calendar-mini");
    var calendarMini = new FullCalendar.Calendar(calendarMiniEl, {
        initialView: "dayGridMonth",
        headerToolbar: {
            end: "today prev,next",
        },
    });
    calendarMini.render();
});

//todo sidebar toggle

const sidebarNavWrapper = document.querySelector("#sidebar-nav-wrapper");
const mainWrapper = document.querySelector("#main-wrapper");
const menuToggleButton = document.querySelector("#menu-toggle");
const menuToggleButtonIcon = document.querySelector("#menu-toggle i");
const overlay = document.querySelector(".overlay");

menuToggleButton.addEventListener("click", () => {
    sidebarNavWrapper.classList.toggle("active");
    overlay.classList.add("active");
    mainWrapper.classList.toggle("active");

    if (document.body.clientWidth > 1200) {
        if (menuToggleButtonIcon.classList.contains("fa-caret-left")) {
            menuToggleButtonIcon.classList.remove("fa-caret-left");
            menuToggleButtonIcon.classList.add("fa-bars");
        }
        else {
            menuToggleButtonIcon.classList.remove("fa-bars");
            menuToggleButtonIcon.classList.add("fa-caret-left");
        }
    } else {
        if (menuToggleButtonIcon.classList.contains("fa-caret-left")) {
            menuToggleButtonIcon.classList.remove("fa-caret-left");
            menuToggleButtonIcon.classList.add("fa-bars");
        }
    }
});
overlay.addEventListener("click", () => {
    sidebarNavWrapper.classList.remove("active");
    overlay.classList.remove("active");
    mainWrapper.classList.remove("active");
});



const navItem = document.querySelectorAll('.nav-item');
let navItemActive = 0;

navItem.forEach((item, i) => {
    item.addEventListener('click', () => {
        navItem[navItemActive].classList.remove('active');
        item.classList.add('active');
        navItemActive = i;
    })
})

