function WordCloud() {
    this.wordNodes = [];
    this.timeStep = 1 / 30.0;
    this.progress = 0;
    this.moveStep = 10;
}

WordCloud.prototype.push = function (n) {
    this.wordNodes.push(n);
};


WordCloud.prototype.findNode = function (id) {
    for (var i = 0; i < this.wordNodes.length; i++) {
        if (this.wordNodes[i].guid == id) {
            return this.wordNodes[i];
        }
    }
    return null;
};

WordCloud.prototype.update = function () {
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
        var acc = [0, 0];
        acc[0] = f[0] / firstNode.weight;
        acc[1] = f[1] / firstNode.weight;
        firstNode.vx += acc[0];
        firstNode.vy += acc[1];
        var speed = lineDistance(0, 0, firstNode.vx, firstNode.vy);
        if (speed > 150) {
            firstNode.vx = 100 * firstNode.vx / speed;
            firstNode.vy = 100 * firstNode.vy / speed;
            speed = 150;
        }
        firstNode.x += (this.timeStep * firstNode.vx + acc[0] * Math.pow(this.timeStep, 2) / 2.0) * this.moveStep;
        firstNode.y += (this.timeStep * firstNode.vy + acc[1] * Math.pow(this.timeStep, 2) / 2.0) * this.moveStep;
    }
    this.updateEnergy();
};

WordCloud.prototype.updateEnergy = function () {
    var energy0 = this.energy;
    this.energy = 0;
    for (var i = 0; i < this.wordNodes.length; i++) {
        var firstNode = this.wordNodes[i];
        for (var j = 0; j < this.wordNodes.length; j++) {
            if (i != j) {
                var secondNode = this.wordNodes[j];
                var dist = 0;
                if (firstNode.type == "point" && secondNode.type == "point") {
                    dist = lineDistance(firstNode.x, firstNode.y, secondNode.x, secondNode.y);
                } else {
                    dist = 1000 * lineDistance(firstNode.x + firstNode.w / 2,
                            firstNode.y + firstNode.h / 2,
                            secondNode.x + secondNode.w / 2,
                            secondNode.y + secondNode.h / 2);
                }
                this.energy += dist;
            }
        }
    }
    this.updateStrengthLength(energy0);
};

WordCloud.prototype.updateStrengthLength = function (energy0) {
    if (this.energy < energy0) {
        this.progress += 1;
        if (this.progress >= 5) {
            this.progress = 0;
            this.moveStep /= 0.9;
        }
    } else {
        this.progress = 0;
        this.moveStep *= 0.9;
    }
};

//cal the repel from the node2 to the node1
WordCloud.prototype.calOverlapRepel = function (node1, node2) {
    var dist = distance(node1, node2) + 0.0001;
    var deltaXY = nodeNodeIntersect(node1, node2);
    var rpl = 0;
    var result = [0, 0];
    if (deltaXY.x > 0 && deltaXY.y > 0) {
        if (node1.type == "point" && node2.type == "point") {
            rpl = -5000 * max(deltaXY.x, deltaXY.y);
        } else if (!(node1.type == "point" && node2.type != "point")) {
            if(node1.semanticNode==node2.semanticNode){
                rpl = -50000 * max(deltaXY.x, deltaXY.y);
            }else {
                rpl = -5000 * max(deltaXY.x, deltaXY.y);
            }
        }
        result = [rpl * (node2.x + node2.w / 2 - node1.x - node1.w / 2) / dist,
            rpl * (node2.y + node2.h / 2 - node1.y - node1.h / 2) / dist];
    }
    return result;
};

WordCloud.prototype.calCenterAttraction = function (node) {
    var dist = lineDistance(node.x + node.w / 2, node.y + node.h / 2, node.semanticNode.x, node.semanticNode.y) + 0.001;
    var atrc = 0;
    if (node.type == "point") {
        atrc = -100 * dist;
    } else {
        atrc = -1000 * dist;
    }
    result = [atrc * (node.x + node.w / 2 - node.semanticNode.x) / dist,
        atrc * (node.y + node.h / 2 - node.semanticNode.y) / dist];
    return result;
};

WordCloud.prototype.resetMoveStep=function () {
    this.moveStep=10;
}
