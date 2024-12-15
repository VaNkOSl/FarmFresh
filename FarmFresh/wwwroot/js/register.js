document.addEventListener('DOMContentLoaded', function () {
    const pass = document.getElementById("password");
    const msg = document.getElementById("message");
    const strengthText = document.getElementById("strength");

    pass.addEventListener('input', () => {

        const password = pass.value;

        const lengthCriteria = password.length >= 8;
        const uppercaseCriteria = /[A-Z]/.test(password);
        const lowercaseCriteria = /[a-z]/.test(password);
        const digitCriteria = /\d/.test(password);
        const specialCharCriteria = /[\W_]/.test(password);

        let strength = "Weak";
        let color = "#ff5925";

        if (lengthCriteria && uppercaseCriteria && lowercaseCriteria && digitCriteria && specialCharCriteria) {
            strength = "Very strong!";
            color = "green";
        } else if (lengthCriteria + (uppercaseCriteria + lowercaseCriteria + digitCriteria + specialCharCriteria >= 3)) {
            strength = "Strong";
            color = "#00c851";
        }
        else if (password.length >= 6 && (uppercaseCriteria || lowercaseCriteria) && digitCriteria) {
            strength = "Medium";
            color = "orange";
        }
        else if (password.length > 0) {
            strength = "Weak";
            color = "#ff5925";
        }
        else {
            strength = "";
        }

        strengthText.textContent = strength;
        pass.style.borderColor = color;
        msg.style.color = color;

        msg.style.display = password.length > 0 ? "block" : "none";
    });
});