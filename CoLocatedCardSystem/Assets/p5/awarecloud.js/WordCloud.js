function WordCloud() {
    this.converged = false;
    this.step = 10;
    this.energy = Number.MAX_VALUE;
    this.wordNodes = new Array();
    this.progress = 0;
    this.optimal = 10;
}

WordCloud.prototype.update = function () {
    var energy0 = this.energy;
    this.energy = 0;
    for (var i = 0; i < this.wordNodes.length; i++) {
        var firstNode = this.wordNodes[i];
        //var fca = [0, 0];
        //var centerAttr = this.calCenterAttraction(firstNode);
        //fca[0] += centerAttr[0];
        //fca[1] += centerAttr[1];
        //var fcalength = Math.sqrt(Math.pow(fca[0], 2) + Math.pow(fca[1], 2));
        //firstNode.x += this.step * (fca[0] / fcalength);
        //firstNode.y += this.step * (fca[1] / fcalength);
        for (var j = i; j < this.wordNodes.length; j++) {
            var f = [0, 0];
            var secondNode = this.wordNodes[j];
            if (i != j) {
                if (secondNode.connect(firstNode)) {
                    var attraction = this.calOverlapAttraction(firstNode, secondNode);
                    f[0] += attraction[0];
                    f[1] += attraction[1];
                }
                var repel = this.calOverlapRepel(firstNode, secondNode);
                f[0] += repel[0];
                f[1] += repel[1];
            }
            var flength = Math.sqrt(Math.pow(f[0], 2) + Math.pow(f[1], 2));
            if (flength > 0) {
                firstNode.x += this.step * (f[0] / flength);
                firstNode.y += this.step * (f[1] / flength);
                secondNode.x -= this.step * (f[0] / flength);
                secondNode.y -= this.step * (f[1] / flength);
            }
            this.energy += Math.pow(flength, 2);
        }
    }
    this.updateStepLength(this.energy, energy0);
}


WordCloud.prototype.calCenterAttraction = function (node) {
    var dist = lineDistance(node.x + node.w / 2, node.y + node.h / 2, node.attrX, node.attrY);
    var atrc = Math.pow(dist, 2);
    result = [atrc * (node.attrX - node.x - node.w / 2) / dist,
        atrc * (node.attrY - node.y - node.h / 2) / dist];
    return result;
}

WordCloud.prototype.calOverlapAttraction = function (node1, node2) {
    var intersect1 = lineRect(
        node1.x + node1.w / 2,
        node1.y + node1.h / 2,
        node2.x + node2.w / 2,
        node2.y + node2.h / 2,
        node1.x,
        node1.y,
        node1.w,
        node1.h
        );
    var intersect2 = lineRect(
        node1.x + node1.w / 2,
        node1.y + node1.h / 2,
        node2.x + node2.w / 2,
        node2.y + node2.h / 2,
        node2.x,
        node2.y,
        node2.w,
        node2.h
    );
    var result = [0, 0];
    var dist = distance(node1, node2);
    if (isSameDirection(intersect2.x - intersect1.x,
        intersect2.y - intersect1.y,
        node2.x - node1.x,
        node2.y - node1.y)) {
        var atrc = lineDistance(intersect1.x, intersect1.y, intersect2.x, intersect2.y) * node1.weight * node2.weight;
        result = [atrc * (node2.x + node2.w / 2 - node1.x - node1.w / 2) / dist,
            atrc * (node2.y + node2.h / 2 - node1.y - node1.h / 2) / dist];
    }
    return result;
}

WordCloud.prototype.calOverlapRepel = function (node1, node2) {
    var dist = distance(node1, node2);
    var deltaXY = rectRect(node1.x, node1.y, node1.w, node1.h, node2.x, node2.y, node2.w, node2.h);
    var rpl = 0;
    var result = [0, 0];
    if (deltaXY.x > 0 && deltaXY.y > 0) {
        rpl = -100000 * min(deltaXY.x, deltaXY.y);
        result = [rpl * (node2.x + node2.w / 2 - node1.x - node1.w / 2) / dist,
                rpl * (node2.y + node2.h / 2 - node1.y - node1.h / 2) / dist];
    }
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