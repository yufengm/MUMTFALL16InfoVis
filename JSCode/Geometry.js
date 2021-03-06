﻿function rectRect(r1x, r1y, r1w, r1h, r2x, r2y, r2w, r2h) {
    var deltax = 0;
    var deltay = 0;
    if (r1x + r1w < r2x) {
        deltax = 0;
    }
    else if (r1x + r1w > r2x && r1x < r2x) {
        deltax = r1x + r1w - r2x
    }
    else if (r1x + r1w < r2x + r2w && r1x > r2x) {
        deltax = r1w;
    }
    else if (r1x + r1w > r2x + r2w && r1x < r2x + r2w) {
        deltax = r2x + r2w - r1x;
    }
    else if (r1x > r2x + r2w) {
        deltax = 0;
    }
    else if (r1x < r2x && r1x + r1w > r2x + r2w) {
        deltax = r2w;
    }
    else if (r1x == r2x && r1x + r1w == r2x + r2w) {
        deltax = r2w;
    }

    if (r1y + r1h < r2y) {
        deltay = 0;
    }
    else if (r1y + r1h > r2y && r1y < r2y) {
        deltay = r1y + r1h - r2y;
    }
    else if (r1y + r1h < r2y + r2h && r1y > r2y) {
        deltay = r1h;
    }
    else if (r1y + r1h > r2y + r2h && r1y < r2y + r2h) {
        deltay = r2y + r2h - r1y;
    }
    else if (r1y > r2y + r2h) {
        deltay = 0;
    }
    else if (r1y < r2y && r1y + r1h > r2y + r2h) {
        deltay = r2h;
    }
    else if (r1y == r2y && r1y + r1h == r2y + r2h) {
        deltay = r2h;
    }
    return { x: deltax, y: deltay };
}


function lineDistance(x1, y1, x2, y2) {
    var result = Math.sqrt(Math.pow(x1 - x2, 2) + Math.pow(y1 - y2, 2));
    return result;
}

function distance(node1, node2) {
    var result = Math.sqrt(Math.pow(node1.x + node1.w / 2 - (node2.x + node2.w / 2), 2)
        + Math.pow(node1.y + node1.h / 2 - (node2.y + node2.h / 2), 2));
    return result;
}

function getTextSize(txt, font, weight) {
    this.element = document.createElement('canvas');
    this.context = this.element.getContext("2d");
    this.context.font = font;
    var tsize = { 'w': this.context.measureText(txt).width + 10, 'h': weight + 10 };
    return tsize;
}