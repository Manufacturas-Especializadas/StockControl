window.readCsvFile = function (fileInput, callback) {
    Papa.parse(fileInput.files[0], {
        complete: function (results) {
            callback(results.data);
        }
    });
};