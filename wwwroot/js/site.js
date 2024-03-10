﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

console.log("hello world");


//start of adapted code Muhib, 2022
// fading animation with better performance
const allSections = document.querySelectorAll(".section");

const fadingSection = (entries, observer) => {
    const [entry] = entries;
    if (!entry.isIntersecting) return;
    entry.target.classList.remove("section_hidden");
    observer.unobserve(entry.target);
};

const sectionObserver = new IntersectionObserver(fadingSection, {
    root: null,
    threshold: 0.12,
});

allSections.forEach((section) => {
    sectionObserver.observe(section);
    section.classList.add("section_hidden");
});

//end of adapted code


const clearIcon = document.querySelector(".clear-icon");
const searchBar = document.querySelector(".search");

searchBar.addEventListener("keyup", () => {
    if (searchBar.value && clearIcon.style.visibility != "visible") {
        clearIcon.style.visibility = "visible";
    } else if (!searchBar.value) {
        clearIcon.style.visibility = "hidden";
    }
});

clearIcon.addEventListener("click", () => {
    searchBar.value = "";
    clearIcon.style.visibility = "hidden";
})



function redirectToMenu() {
        document.getElementById("menuForm").submit();
    }
function redirectToContact() {
    document.getElementById("contactForm").submit();
}
function redirectToPay() {
    document.getElementById("payForm").submit();
}

function submitForm() {
    document.getElementById("form").submit();
}

