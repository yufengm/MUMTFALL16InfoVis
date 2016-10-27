function Node() {
    this.connections = [];
    this.txt = "test";
    this.id = 0;
    this.x = 0;
    this.y = 0;
    this.txtSize = 20;
    this.velocityX = 0;
    this.velocityY = 0;
}

Node.prototype.connect = function (node) {
    if (this.connections.length === 0) {
        return false;
    } else {
        for (ci = 0; ci < this.connections.length; ci++) {
            if (this.connections[ci].id == node.id) {
                ci = 0;
                return true;
            }
        }
        return false;
    }
}