// OTP Timer & Resend By Abolfazl Miri 
let timerDuration = 120;
let timerDisplay = document.getElementById("timer");
let resendBtn = document.getElementById("resendBtn");
let userPhoneNumber = document.getElementById("Mobile");
let phoneNumber = userPhoneNumber.value;
function startTimer() {
    resendBtn.disabled = true;
    let timeLeft = timerDuration;

    let interval = setInterval(() => {
        let minutes = Math.floor(timeLeft / 60);
        let seconds = timeLeft % 60;
        timerDisplay.textContent = `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;

        if (timeLeft <= 0) {
            clearInterval(interval);
            resendBtn.disabled = false;
            timerDisplay.textContent = "00:00";
        }

        timeLeft--;
    }, 1000);
}

async function resendOtp() {
    try {
        resendBtn.disabled = true;
        const response = await fetch("/resend-otp", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ mobile: phoneNumber })
        });

        if (!response.ok) throw new Error("Network response was not ok");

        const data = await response.json();
        ShowMessage('پیغام', data.message, 'info');
        startTimer();
    } catch (err) {
        console.error(err);
        ShowMessage('خطا', 'ارسال کد با خطا مواجه شد.', 'error');
        resendBtn.disabled = false;
    }
}

// Event listener
resendBtn.addEventListener("click", resendOtp);

// Automatically start timer on page load
document.addEventListener("DOMContentLoaded", () => {
    startTimer(120);
});