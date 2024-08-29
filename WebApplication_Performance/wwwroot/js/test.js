// For bundling and minification

document.addEventListener("DOMContentLoaded", function () {
    console.log("Welcome to the Library Application!");

    document.getElementById("myButton").addEventListener("click", function () {
        alert("Button was clicked!");
    });
});

function validateForm() {
    let input = document.getElementById("myInput").value;
    if (input === "") {
        alert("Input cannot be empty");
        return false;
    }
    return true;
}

function changeText(elementId, newText) {
    document.getElementById(elementId).innerText = newText;
}

function toggleDarkMode() {
    document.body.classList.toggle("dark-mode");
}