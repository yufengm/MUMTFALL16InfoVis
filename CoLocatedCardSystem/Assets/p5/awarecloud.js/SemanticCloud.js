/**
 * Created by Shuo on 11/19/2016.
 */
function SemanticCloud() {
    this.semanticNodes = [];
    this.timeStep = 1 / 30.0;
    this.moveStep = 10;
    this.progress = 0;
    this.optimal = 60;
    this.energy=0;
}


SemanticCloud.prototype.push = function (n) {
    this.semanticNodes.push(n);
};

SemanticCloud.prototype.findNode = function (id) {
    for (var i = 0; i < this.semanticNodes.length; i++) {
        if (this.semanticNodes[i].guid == id) {
            return this.semanticNodes[i];
        }
    }
    return null;
};

SemanticCloud.prototype.update = function () {
    var energy0 = this.energy;
    this.energy = 0;
    var center = [0, 0];
    for (var i = 0; i < this.semanticNodes.length; i++) {
        var firstNode = this.semanticNodes[i];
        var f = [0, 0];
        for (var j = 0; j < this.semanticNodes.length; j++) {
            if (i != j) {
                var secondNode = this.semanticNodes[j];
                var repel = this.calRepel(firstNode, secondNode);
                f[0] += repel[0];
                f[1] += repel[1];
            }
        }
        for (var j = 0; j < firstNode.connections.length; j++) {
            var secondNode = firstNode.connections[j];
            var attraction = this.calAttraction(firstNode, secondNode);
            f[0] += attraction[0];
            f[1] += attraction[1];
        }
        var borderRepel = this.calBorderRepel(firstNode);
        f[0] += borderRepel[0];
        f[1] += borderRepel[1];
        var acc = [0, 0];
        acc[0] = f[0] / firstNode.weight;
        acc[1] = f[1] / firstNode.weight;
        firstNode.vx += acc[0] / this.timeStep;
        firstNode.vy += acc[1] / this.timeStep;
        var speed = lineDistance(0, 0, firstNode.vx, firstNode.vy);
        if (speed > 10) {
            firstNode.vx = 10 * firstNode.vx / speed;
            firstNode.vy = 10 * firstNode.vy / speed;
            speed = 10;
        }
        firstNode.x += (this.timeStep * firstNode.vx + acc[0] * Math.pow(this.timeStep, 2) / 2.0) * this.moveStep;
        center[0] += firstNode.x;
        firstNode.y += (this.timeStep * firstNode.vy + acc[1] * Math.pow(this.timeStep, 2) / 2.0) * this.moveStep;
        center[1] += firstNode.y;
        var fLength = lineDistance(0, 0, f[0], f[1]);
        this.energy += fLength * fLength;
    }
    center[0] = canvasWidth / 2 - center[0] / this.semanticNodes.length;
    center[1] = canvasHeight / 2 - center[1] / this.semanticNodes.length;
    for (var i = 0; i < this.semanticNodes.length; i++) {
        var firstNode = this.semanticNodes[i];
        firstNode.x += center[0];
        firstNode.y += center[1];
    }
    this.updateStrengthLength(energy0);
};

SemanticCloud.prototype.updateStrengthLength = function (energy0) {
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

SemanticCloud.prototype.calAttraction = function (node1, node2) {
    var dist = lineDistance(node1.x, node1.y, node2.x, node2.y);
    var atrc = dist * dist / this.optimal;
    var result = [atrc * (node2.x - node1.x ) / dist,
        atrc * (node2.y - node1.y ) / dist];
    return result;
};

SemanticCloud.prototype.calRepel = function (node1, node2) {
    var dist = lineDistance(node1.x, node1.y, node2.x, node2.y);
    var rpl = -10 * this.optimal * this.optimal / dist;
    var result = [rpl * (node2.x - node1.x ) / dist,
        rpl * (node2.y - node1.y ) / dist];
    return result;
};

SemanticCloud.prototype.calBorderRepel = function (node) {
    var dist = lineDistance(node.x, node.y, canvasWidth / 2, canvasHeight / 2) + 0.001;
    var atrc = 0;
    var result = [0, 0];
    if (node.x < canvasWidth/10 || node.x > canvasWidth-100 || node.y < canvasHeight/10 || node.y > canvasHeight-100) {
        atrc = -30000;
    }
    var result = [atrc * (node.x - canvasWidth / 2) / dist,
        atrc * (node.y - canvasHeight / 2) / dist];
    return result;
};

SemanticCloud.prototype.resetMoveStep=function () {
    this.moveStep=10;
}