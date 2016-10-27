function WordCloud() {
    this.converged = false;
    this.step = 10;
    this.energy = Number.MAX_VALUE;
    this.wordNodes = new Array();
    this.step = 0.05;
    this.progress = 0;
    this.optimal = 50;
}

WordCloud.prototype.update = function () {
    var energy0 = this.energy;
    this.energy = 0;
    for (var i = 0; i < this.wordNodes.length; i++) {
        var f = [0, 0];
        var curNode = this.wordNodes[i];
        for (var j = 0; j < this.wordNodes.length; j++) {
            var indexNode = this.wordNodes[j];
            if (indexNode.connect(curNode)) {
                var attraction = this.calAttraction(curNode, indexNode);
                f[0] += attraction[0];
                f[1] += attraction[1];
            }
        }
        for (var j = 0; j < this.wordNodes.length; j++) {
            if (i != j) {
                var indexNode = this.wordNodes[j];
                var repel = this.calRepel(curNode, indexNode);
                f[0] += repel[0];
                f[1] += repel[1];
            }
        }
        var flength = Math.sqrt(Math.pow(f[0], 2) + Math.pow(f[1], 2));
        curNode.x += this.step * (f[0] / flength);
        curNode.y += this.step * (f[1] / flength);
        this.energy += Math.pow(flength, 2);
    }
    this.updateStepLength(this.energy, energy0);
}

WordCloud.prototype.calAttraction = function (node1, node2) {
    var dist = distance(node1, node2);
    var atrc = Math.pow(dist, 2) / this.optimal;
    var result = [atrc * (node2.x - node1.x) / dist, atrc * (node2.y - node1.y) / dist];
    return result;
}

WordCloud.prototype.calRepel = function (node1, node2) {
    var dist = distance(node1, node2);
    var rpl = -1 * Math.pow(this.optimal, 2) / dist;
    var result = [rpl * (node2.x - node1.x) / dist, rpl * (node2.y - node1.y) / dist];
    return result;
}

WordCloud.prototype.updateStepLength = function (eng, eng0) {
    if (eng < eng0) {
        this.progress = this.progress + 1;
        if (this.progress >= 5) {
            this.progress = 0;
            this.step = this.step / 0.9;
        }
    } else {
        this.progress = 0;
        this.step = 0.9 * this.step;
    }
}

WordCloud.prototype.push = function (n) {
    this.wordNodes.push(n);
}