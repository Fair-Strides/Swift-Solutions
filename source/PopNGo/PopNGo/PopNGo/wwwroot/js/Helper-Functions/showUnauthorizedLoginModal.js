

function showLoginSignupModal() {
    const modal = document.getElementById('unauthorized-login-modal');
    if (!modal) {
        console.error('Modal element #unauthorized-login-modal not found.');
        return;
    }
    modal.style.display = 'block';
    document.body.style.overflow = 'hidden'; // Prevent scrolling

    const span = document.getElementsByClassName("close")[0];
    span.onclick = function () { // Close the modal if the user clicks on the close button
        modal.style.display = 'none';
        document.body.style.overflow = 'auto'; // Restore scrolling
    }

    window.onclick = function (event) { // Close the modal if the user clicks outside of it
        if (event.target == modal) {
            modal.style.display = 'none';
            document.body.style.overflow = 'auto';
        }
    }
}

export { showLoginSignupModal };