function Node() {
    this.connections = [];
    this.txt = "test";
    this.group = "";
    this.x = 0;
    this.y = 0;
    this.attrX = 0;
    this.attrY = 0;
    this.weight = 20;
    this.w = 0;
    this.h = 0;
}

Node.prototype.connect = function (node) {
    if (this.connections.length === 0) {
        return false;
    } else {
        for (var i = 0; i < this.connections.length; i++) {
            if (this.connections[i].txt == node.txt && this.connections[i].group == node.group) {
                return true;
            }
        }
        return false;
    }
}


