//https://github.com/niklasvh/html2canvas/issues/722

function put2Canvas(canvasDestination) {
    html2canvas(document.querySelector("#spieldruck")).then(canvas => {

        var canvasID = document.getElementById(canvasDestination);
        //get the destination context
        var ctx = canvasID.getContext('2d');
        ctx.scale(0.5, 0.5);
        ctx.width = window.innerWidth / 3;
        ctx.height = window.innerHeight / 3;
        ctx.drawImage(canvas, 0, 0);
    });
}


function getData() {
    return new Promise((resolve, reject) => {
        html2canvas(document.querySelector("#spieldruck"), {
            logging: true, letterRendering: 1, allowTaint: false, useCORS: true
        }).then(canvas => {
            resolve(canvas.toDataURL())
        })
    });
}


function downloadScreenShot(filename) {
    html2canvas(document.querySelector("#spieldruck")).then(canvas => {
        saveAs(canvas.toDataURL(), filename + '.png')
    });
}


function saveAs(uri, filename) {
    var link = document.createElement('a');
    if (typeof link.download === 'string') {
        link.href = uri;
        link.download = filename;

        //Firefox requires the link to be in the body
        document.body.appendChild(link);

        //simulate click
        link.click();

        //remove the link when done
        document.body.removeChild(link);
    } else {
        window.open(uri);
    }
}