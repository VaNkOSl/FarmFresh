document.addEventListener('DOMContentLoaded', function () {
    const payWithCardRadio = document.getElementById('payWithCard');
    const payOnDeliveryRadio = document.getElementById('payOnDelivery');
    const orderId = document.getElementById('id').value;

    payWithCardRadio.addEventListener('change', function () {
        if (payWithCardRadio.checked) {
            window.location.href = `/api/payment/processcardpayment/${orderId}`;

        }
    });

    payOnDeliveryRadio.addEventListener('change', function () {
        if (payOnDeliveryRadio.checked) {
            console.log("Pay on Delivery selected");
        }
    });
});