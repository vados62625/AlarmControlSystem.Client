window.saveComment = (id, text) => {
    var x = document.getElementById("snackbar");
    if (text) 
    {
        x.textContent = text;
    }
    x.className = "show";
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 2000);
}

window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}

window.scrollChatToBottom = () => {
    var objDiv = document.getElementById("message-story-container");
    objDiv.scrollTop = objDiv.scrollHeight;
    // objDiv.scrollIntoView(false);
}