const dropArea = document.getElementById('drop-area');
const fileInput = document.getElementById('images');
const preview = document.getElementById('preview');

let droppedFiles = [];

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
    droppedFiles = [...droppedFiles, ...fileArray];

    updateFileInput();
    displayPreview(fileArray);
}

function updateFileInput() {
    const dataTransfer = new DataTransfer();
    droppedFiles.forEach((file) => dataTransfer.items.add(file));
    fileInput.files = dataTransfer.files;
}

function displayPreview(files) {
    files.forEach((file, index) => {
        if (file.type.startsWith('image/')) {
            const reader = new FileReader();

            reader.onload = function (e) {
                const previewContainer = document.createElement('div');
                previewContainer.className = 'preview-item';

                const imgElement = document.createElement('img');
                imgElement.src = e.target.result;
                imgElement.alt = file.name;

                const removeButton = document.createElement('button');
                removeButton.textContent = 'X';
                removeButton.className = 'remove-button';
                removeButton.addEventListener('click', () => removeFile(index, previewContainer));

                previewContainer.appendChild(imgElement);
                previewContainer.appendChild(removeButton);
                preview.appendChild(previewContainer);
            };

            reader.readAsDataURL(file);
        } else {
            alert('Only image files are allowed!');
        }
    });
}

function removeFile(index, previewContainer) {
    droppedFiles.splice(index, 1); 
    updateFileInput(); 
    previewContainer.remove(); 
}
