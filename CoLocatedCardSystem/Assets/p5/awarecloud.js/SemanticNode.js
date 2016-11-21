/**
 * Created by Shuo on 11/19/2016.
 */
function SemanticNode() {
    this.guid = "";
    this.connections = [];
    this.wordNodes = [];
    this.semantic = "test";
    this.owner = "";
    this.nodeColor = [145, 170, 157];
    this.x = 0.0;
    this.y = 0.0;
    this.vx = 0;
    this.vy = 0;
    this.weight = 10;
}

SemanticNode.prototype.removeWordNode = function (node) {
    var tobeRemoved;
    for (var i = 0; i < this.wordNodes.length; i++) {
        if (this.wordNodes[i].guid == node.guid) {
            tobeRemoved = i;
        }
    }
    if (tobeRemoved != null) {
        this.wordNodes.splice(tobeRemoved, 1);
    }
};

SemanticNode.prototype.connect = function (semanticNode) {
    for (var i = 0; i < this.connections.length; i++) {
        if (this.connections[i].guid == semanticNode.guid) {
            return;
        }
    }
    this.connections.push(semanticNode);
};
