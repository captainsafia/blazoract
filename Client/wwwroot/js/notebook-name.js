const registerListener = (element, component) => {
    element.addEventListener("input", function (event) {
        component.invokeMethodAsync('RenameNotebook', element.innerText);
    });
};

export {
    registerListener
}