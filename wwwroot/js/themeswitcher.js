window.applyThemeSwitcher = () => {
    let t = window.localStorage.getItem("blazorbootstrap-theme");
    if (!t || t === "system"){
        t = window.matchMedia(`(prefers-color-scheme: dark)`).matches ? "dark" : "light";
    }
    document.documentElement.setAttribute('data-bs-theme', t);
}