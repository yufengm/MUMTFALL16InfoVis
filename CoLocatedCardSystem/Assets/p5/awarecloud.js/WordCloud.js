function WordCloud() {
    this.wordNodes = [];
    this.timeStep = 1 / 30.0;
    this.moveStep = 1;
    this.progress = 0;
}

WordCloud.prototype.push = function (n) {
    this.wordNodes.push(n);
}

WordCloud.prototype.removeNode = function (text, group) {
    var tobeRemoved;
    for (var i = 0; i < this.wordNodes.length; i++) {
        if (this.wordNodes[i].stemmedText == text && this.wordNodes[i].group == group) {
            tobeRemoved = i;
        }
    }
    if (tobeRemoved != null) {
        this.wordNodes.splice(tobeRemoved, 1);
    }
    return null;
}

WordCloud.prototype.findNode = function (text, group) {
    for (var i = 0; i < this.wordNodes.length; i++) {
        //if (this.wordNodes[i].stemmedText == text && this.wordNodes[i].group == group) {
        //    return this.wordNodes[i];
        //}
        if (this.wordNodes[i].stemmedText == text) {
            return this.wordNodes[i];
        }
    }
    return null;
}

WordCloud.prototype.update = function () {
    var energy0 = this.energy;
    this.energy = 0;
    for (var i = 0; i < this.wordNodes.length; i++) {
        var firstNode = this.wordNodes[i];
        var f = [0, 0];
        for (var j = 0; j < this.wordNodes.length; j++) {
            if (i != j) {
                var secondNode = this.wordNodes[j];
                var repel = this.calOverlapRepel(firstNode, secondNode);
                f[0] += repel[0];
                f[1] += repel[1];
            }
        }
        var attraction = this.calCenterAttraction(firstNode);
        f[0] += attraction[0];
        f[1] += attraction[1];
        this.energy += f[0] * f[0] + f[1] * f[1];
        var acc = [0, 0];
        acc[0] = f[0] / firstNode.weight;
        acc[1] = f[1] / firstNode.weight;
        firstNode.vx += acc[0] / this.timeStep;
        firstNode.vy += acc[1] / this.timeStep;
        var speed = lineDistance(0, 0, firstNode.vx, firstNode.vy);
        if (speed > 200) {
            firstNode.vx = 200 * firstNode.vx / speed;
            firstNode.vy = 200 * firstNode.vy / speed;
            speed = 200;
        }
        firstNode.x += (this.timeStep * firstNode.vx + acc[0] * Math.pow(this.timeStep, 2) / 2.0) * this.moveStep;
        firstNode.y += (this.timeStep * firstNode.vy + acc[1] * Math.pow(this.timeStep, 2) / 2.0) * this.moveStep;
        this.energy += firstNode.weight * speed * speed;
    }
    for (var i = 0; i < this.wordNodes.length; i++) {
        var firstNode = this.wordNodes[i];
        var closest = Number.MAX_VALUE;
        for (var j = 0; j < this.wordNodes.length; j++) {
            if (i != j) {
                var secondNode = this.wordNodes[j];
                var dist = lineDistance(firstNode.x, firstNode.y, secondNode.x, secondNode.y);
                if (dist < closest) {
                    closest = dist;
                }
            }
        }
        if (closest > 80) {
            this.moveStep = 1;
        }
    }
    this.updateStrengthLength(energy0);
}

WordCloud.prototype.updateStrengthLength = function (energy0) {
    if (this.energy < energy0) {
        this.progress += 1;
        if (this.progress >= 5) {
            this.progress = 0;
            this.moveStep /= 0.95;
        }
    } else {
        this.progress = 0;
        this.moveStep *= 0.95;
    }
}

WordCloud.prototype.calOverlapRepel = function (node1, node2) {
    var dist = distance(node1, node2) + 0.0001;
    var deltaXY = rectRect(node1, node2);
    var rpl = 0;
    var result = [0, 0];
    if (deltaXY.x > 0 && deltaXY.y > 0) {
        if (node1.type == "point" && node2.type == "point") {
            rpl = -40000 * max(deltaXY.x, deltaXY.y);
        } else {
            rpl = -5000 * max(deltaXY.x, deltaXY.y);
        }
        result = [rpl * (node2.x + node2.w / 2 - node1.x - node1.w / 2) / dist,
            rpl * (node2.y + node2.h / 2 - node1.y - node1.h / 2) / dist];
    }
    return result;
}

WordCloud.prototype.calCenterAttraction = function (node) {
    var dist = lineDistance(node.x + node.w / 2, node.y + node.h / 2, node.attrX, node.attrY) + 0.001;
    if (node.type == "point") {
        atrc = -100 * dist;
    } else {
        atrc = -100 * dist;
    }
    result = [atrc * (node.x + node.w / 2 - node.attrX) / dist,
        atrc * (node.y + node.h / 2 - node.attrY) / dist];
    return result;
}


