function WordCloud() {
    this.wordNodes = new Array();
    this.timeStep = 1 / 30.0;
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
        if (this.wordNodes[i].stemmedText == text && this.wordNodes[i].group == group) {
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

        var a = [0, 0];
        a[0] = f[0] / firstNode.weight;
        a[1] = f[1] / firstNode.weight;
        firstNode.vx = (firstNode.vx + a[0] / this.timeStep) * 0.01;
        firstNode.vy = (firstNode.vy + a[1] / this.timeStep) * 0.01;
        firstNode.x = firstNode.x + this.timeStep * firstNode.vx + a[0] * Math.pow(this.timeStep, 2) / 2.0;
        firstNode.y = firstNode.y + this.timeStep * firstNode.vy + a[1] * Math.pow(this.timeStep, 2) / 2.0;
    }
    //this.updateStepLength(this.energy, energy0);
}

WordCloud.prototype.calOverlapRepel = function (node1, node2) {
    var dist = distance(node1, node2) + 0.0001;
    var deltaXY = rectRect(node1.x, node1.y, node1.w, node1.h, node2.x, node2.y, node2.w, node2.h);
    var rpl = 0;
    var result = [0, 0];
    if (deltaXY.x > 0 && deltaXY.y > 0) {
        rpl = -1000 * min(deltaXY.x, deltaXY.y);
        result = [rpl * (node2.x + node2.w / 2 - node1.x - node1.w / 2) / dist,
                rpl * (node2.y + node2.h / 2 - node1.y - node1.h / 2) / dist];
    }
    return result;
}

WordCloud.prototype.calCenterAttraction = function (node) {
    var dist = lineDistance(node.x + node.w / 2, node.y + node.h / 2, node.attrX, node.attrY) + 0.001;
    var atrc = -100 * dist;
    result = [atrc * (node.x + node.w / 2 - node.attrX) / dist,
        atrc * (node.y + node.h / 2 - node.attrY) / dist];
    return result;
}


