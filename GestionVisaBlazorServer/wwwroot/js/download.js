// Fonction pour générer un PDF avec jsPDF et autoTable (titre et nom de fichier dynamiques)
window.GeneratePDF = function (options) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    const { header, data, title, fileName } = options;

    doc.setFontSize(14);
    doc.text(title || "Liste", 14, 15);

    doc.autoTable({
        head: [header],
        body: data,
        startY: 20,
        theme: 'grid',
        styles: { fontSize: 10 }
    });

    doc.save(fileName || "export.pdf");
};

// Fonction de téléchargement pour un fichier depuis base64
window.BlazorDownloadFile = (fileName, contentType, base64Data) => {
    const link = document.createElement('a');
    link.download = fileName;
    link.href = `data:${contentType};base64,${base64Data}`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};
