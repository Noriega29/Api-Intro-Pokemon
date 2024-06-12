// Definir una función llamada activateInputTipo2
function activateInputTipo2() {
    // Obtener los elementos con id "check", "select" y guardarlo en variables
    var checkTipo2 = document.getElementById("checkTipo2");
    var tipo2 = document.getElementById("tipo2")
    // Si el elemento check está marcado
    if (checkTipo2.checked) {
        // Habilitar el elemento select
        tipo2.disabled = false;
    }
    // Si no
    else {
        // Deshabilitar el elemento select
        tipo2.disabled = true;
    }
}

// Example starter JavaScript for disabling form submissions if there are invalid fields
(() => {
    'use strict'

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    const forms = document.querySelectorAll('.needs-validation')

    // Loop over them and prevent submission
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault()
                event.stopPropagation()
            }

            form.classList.add('was-validated')
        }, false)
    })
})()
