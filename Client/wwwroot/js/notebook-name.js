const registerListener = (element) => {
    element.addEventListener("input", function (event) {
        DotNet.invokeMethodAsync("blazoract.Client", "RenameNotebook", element.innerText)
    });
};

export {
    registerListener
}