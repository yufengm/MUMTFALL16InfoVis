function Node() {
    this.connections = [];
    this.type = 0;
    this.txt = "test";
    this.stemmedText = "test";
    this.group = "";
    this.x = 0;
    this.y = 0;
    this.vx = 10;
    this.vy = 10;
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
            if (this.connections[i].stemmedText == node.stemmedText && this.connections[i].group == node.group) {
                return true;
            }
        }
        return false;
    }
}


