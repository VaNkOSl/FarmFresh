const dropArea = document.getElementById('drop-area');
const fileInput = document.getElementById('images');
const preview = document.getElementById('preview');

dropArea.addEventListener('click', () => {
    fileInput.click();
});

dropArea.addEventListener('dragover', (e) => {
    e.preventDefault();
    dropArea.style.backgroundColor = "#e9ecef"; 
});

dropArea.addEventListener('dragleave', () => {
    dropArea.style.backgroundColor = "#f8f9fa";
});

dropArea.addEventListener('drop', (e) => {
    e.preventDefault();
    handleFiles(e.dataTransfer.files);
});

fileInput.addEventListener('change', () => {
    handleFiles(fileInput.files);
});

function handleFiles(files) {
    const fileArray = Array.from(files);

    fileArray.forEach((file) => {
        if (file.type.startsWith('image/')) {
            const reader = new FileReader();

            reader.onload = function (e) {
                const imgElement = document.createElement('img');
                imgElement.src = e.target.result;
                preview.appendChild(imgElement);
            };

            reader.readAsDataURL(file);
        } else {
            alert('Only image files are allowed!');
        }
    });
}