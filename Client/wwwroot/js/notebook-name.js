const registerListener = (element) => {
    console.log("resgiter listener");
    element.addEventListener("input", function (event) {
        DotNet.invokeMethodAsync("blazoract.Client", "RenameNotebook", element.innerText)
    });
};

export {
    registerListener
}