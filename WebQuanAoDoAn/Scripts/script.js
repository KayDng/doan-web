//todo: Wrapper picture

const bar = document.getElementById('bar');
const nav = document.getElementById('navbar');
const close = document.getElementById('close');

if (bar) {
    bar.addEventListener('click', () => {
        nav.classList.add('active');
    })
}

if (close) {
    close.addEventListener('click', () => {
        nav.classList.remove('active');
    })
}

//todo: PASSWORD

const password = document.getElementById('pwd');
const pwd_signup = document.getElementById('pwd-signup');
const pwd_signup_again = document.getElementById('pwd-signup-again');
const toggle = document.getElementById('toggle');
const toggle_signup = document.getElementById('toggle-signup');
const toggle_signup_again = document.getElementById('toggle-signup-again');

function showPassword() {
    if (password.type === 'password') {
        password.setAttribute('type', 'text');
        toggle.classList.add('hide');
    }
    else {
        password.setAttribute('type', 'password');
        toggle.classList.remove('hide');
    }
}

function showPasswordSingup() {
    if (pwd_signup.type === 'password') {
        pwd_signup.setAttribute('type', 'text');
        toggle_signup.classList.add('hide-signup');
    }
    else {
        pwd_signup.setAttribute('type', 'password');
        toggle_signup.classList.remove('hide-signup');
    }
}

function showPassword() {
    if (password.type === 'password') {
        password.setAttribute('type', 'text');
        toggle.classList.add('hide');
    }
    else {
        password.setAttribute('type', 'password');
        toggle.classList.remove('hide');
    }
}

function notification() {
    if (password.type === 'password') {
        password.setAttribute('type', 'text');
        toggle.classList.add('hide');
    }
    else {
        password.setAttribute('type', 'password');
        toggle.classList.remove('hide');
    }
}

//todo: toggle size button

const sizeBtns = document.querySelectorAll('.size-radio-btn');
let checkedBtn = 0;

sizeBtns.forEach((item, i) => {
    item.addEventListener('click', () => {
        sizeBtns[checkedBtn].classList.remove('check');
        item.classList.add('check');
        checkedBtn = i;
    })
})

//todo: toggl Image Slide

const productImages = document.querySelectorAll(".product-container .product-gallery-container img");
const productImageSlide = document.querySelector(".image-slider");

let activeImageSlide = 0;

productImages.forEach((item, i) => {
    item.addEventListener('click', () => {
        productImages[activeImageSlide].classList.remove('actives');
        item.classList.add('actives');
        productImageSlide.style.backgroundImage = `url('${item.src}')`;
        activeImageSlide = i;
    })
})

//todo: password
function onChange() {
    const password = document.querySelector('input[name=pwd-signup]');
    const confirm = document.querySelector('input[name=pwd-signup-again]');
    if (confirm.value === password.value) {
        confirm.setCustomValidity('');
    } else {
        confirm.setCustomValidity('Passwords do not match');
    }
}

// js cho nút + -
var btntru = document.querySelectorAll('.jstru')
var btncong = document.querySelectorAll('.jscong')

btncong.forEach(function (ptu, index) {
    ptu.onclick = function () {
        document.querySelectorAll(".item-quantity")[index].value++;
    }
})
btntru.forEach(function (ptu, index) {
    ptu.onclick = function () {
        if (document.querySelectorAll(".item-quantity")[index].value > 1) {
            document.querySelectorAll(".item-quantity")[index].value--;
        }
        if (document.querySelectorAll(".item-quantity")[index].value == "") {
            document.querySelectorAll(".item-quantity")[index].value = 1;
        }
    }
})
// js ko cho nhap so
$(document).ready(function () {
    $('.js-nostring').bind("cut copy paste drag drop", function (e) {
        e.preventDefault();
    });
});
function isNumberKey(e) {
    var charCode = (e.which) ? e.which : e.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}