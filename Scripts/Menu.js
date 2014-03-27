function menu_onmouseover (n_id) {
    document.getElementById(n_id).className = "menuOver";
}

function menu_onmouseout (n_id) {

    document.getElementById(n_id).className = "menuOut";

}

function menu_onselected (n_id) {
    document.getElementById(n_id).className = "menuOutBold";
}