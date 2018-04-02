function prevNextLinks(mon, yr, cont) {
    var prev = document.getElementById('previous');
    var next = document.getElementById('next');

    var prevm = mon;
    var nextm = mon;
    var prevy = yr;
    var nexty = yr;

    if (mon == 1) {
        prevm = 12;
        nextm++;
        prevy--;
    }
    else if (mon == 12) {
        nextm = 1;
        nexty++;
        prevm--;
    }
    else {
        prevm--;
        nextm++;
    }
    prev.setAttribute('href', '/' + cont + '/Index?m=' + prevm + '&y=' + prevy + '&fm=' + prevm + '&fy=' + prevy);
    next.setAttribute('href', '/' + cont + '/Index?m=' + nextm + '&y=' + nexty + '&fm=' + nextm + '&fy=' + nexty);
}