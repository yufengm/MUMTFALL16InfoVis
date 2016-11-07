function lineRect(x1, y1, x2, y2, rx, ry, rw, rh) {
    // check if the line has hit any of the rectangle's sides
    // uses the Line/Line function below
    var left = lineLine(x1, y1, x2, y2, rx, ry, rx, ry + rh);
    var right = lineLine(x1, y1, x2, y2, rx + rw, ry, rx + rw, ry + rh);
    var top = lineLine(x1, y1, x2, y2, rx, ry, rx + rw, ry);
    var bottom = lineLine(x1, y1, x2, y2, rx, ry + rh, rx + rw, ry + rh);

    // if ANY of the above are true, the line
    // has hit the rectangle
    if (left.x != -1 && left.y != -1) {
        return left;
    }
    if (right.x != -1 && right.y != -1) {
        return right;
    }
    if (top.x != -1 && top.y != -1) {
        return top;
    }
    if (bottom.x != -1 && bottom.y != -1) {
        return bottom;
    }
    return { x: -1, y: -1 };
}

function lineLine(x1, y1, x2, y2, x3, y3, x4, y4) {
    // calculate the distance to intersection point
    var uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
    var uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

    // if uA and uB are between 0-1, lines are colliding
    if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1) {

        // optionally, draw a circle where the lines meet
        var intersectionX = x1 + (uA * (x2 - x1));
        var intersectionY = y1 + (uA * (y2 - y1));
        return { x: intersectionX, y: intersectionY };
    }
    return { x: -1, y: -1 };
}

function rectRect(r1x, r1y, r1w, r1h, r2x, r2y, r2w, r2h) {
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

function isSameDirection(x1, y1, x2, y2) {
    var value = x1 * x2 + y1 * y2;
    return value > 0;
}

function lineDistance(x1, y1, x2, y2) {
    var result = Math.sqrt(Math.pow(x1 - x2, 2) + Math.pow(y1 - y2, 2));
    return result;
}

function distance(node1, node2) {
    var result = Math.sqrt(Math.pow(node1.x + node1.w / 2 - (node2.x + node2.w / 2), 2)
        + Math.pow(node1.y + node1.h / 2 - (node2.y + node2.h / 2), 2));
    if (result == 0) {
        return 0.1;
    }
    return result;
}

function getTextSize(txt, font, weight) {
    this.element = document.createElement('canvas');
    this.context = this.element.getContext("2d");
    this.context.font = font;
    var tsize = { 'w': this.context.measureText(txt).width, 'h': weight / 1.5 };
    return tsize;
}