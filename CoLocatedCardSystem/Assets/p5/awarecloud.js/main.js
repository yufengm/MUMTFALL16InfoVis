var wordCloud;
var semanticCloud;
function setup() {
    frameRate(30);
    createCanvas(windowWidth, windowHeight);
    background(255);
    wordCloud = new WordCloud();
    semanticCloud = new SemanticCloud();
    randomSeed(1);
    // for (var i = 0; i < 5; i++) {
    //     addSemanticNode("node" + i, "test");
    //     setSemanticNodeColor("node" + i, random(255), random(255), random(255));
    // }
    // connectSemanticNode("node0", "node1");
    // connectSemanticNode("node0", "node2");
    // connectSemanticNode("node0", "node3");
    // connectSemanticNode("node3", "node4");
}

function draw() {
    background(25, 52, 65);
    wordCloud.update();
    semanticCloud.update();
    //for debug
    stroke(255);
    noFill();
    for (var i = 0; i < semanticCloud.semanticNodes.length; i++) {
        var node = semanticCloud.semanticNodes[i];
        ellipse(node.x, node.y, 10, 10);
        for (var j = 0; j < node.connections.length; j++) {
            line(node.x, node.y, node.connections[j].x, node.connections[j].y);
        }

    }
    //draw image
    fill(252, 255, 245);
    noStroke();
    for (var i = 0; i < wordCloud.wordNodes.length; i++) {
        var node = wordCloud.wordNodes[i];
        if (node.type == "image") {
            image(node.image, node.x + 5, node.y + 5, node.w, node.h);
        }
    }
    //draw dots
    for (var i = 0; i < wordCloud.wordNodes.length; i++) {
        var node = wordCloud.wordNodes[i];
        if (node.type == "point") {
            stroke(255, 255, 0);
            fill(node.semanticNode.nodeColor[0], node.semanticNode.nodeColor[1], node.semanticNode.nodeColor[2]);
            ellipse(node.x + node.w / 2, node.y + node.w / 2, node.weight, node.weight);
        }
    }
    //draw text
    noStroke();
    for (var i = 0; i < wordCloud.wordNodes.length; i++) {
        var node = wordCloud.wordNodes[i];
        if (node.type == "text") {
            fill(node.semanticNode.nodeColor[0], node.semanticNode.nodeColor[1], node.semanticNode.nodeColor[2]);
            textSize(node.weight);
            text(node.cloudText, node.x + 2, node.y + node.h - 2);
            //rect(node.x, node.y, node.w, node.h);
        }
    }

    //debug;
    fill(255);
    noStroke();
    textSize(15);
    text("node# " + wordCloud.wordNodes.length, 10, 20);
    text("Energy " + wordCloud.energy, 10, 35);
    text("Step" + wordCloud.moveStep, 10, 50);
}

function mouseClicked() {
    resetMoveStep();
}

// function mouseMoved() {//for debug
//     var newid = "point" + Math.trunc(random(200));
//     sid = "node" + Math.trunc(random(semanticCloud.semanticNodes.length));
//     createWordNode(newid, "point", sid);
//     //updateWordNodePosition(newid, mouseX, mouseY);
//     //setWordNodeWeight(newid,20);
//     newid = "text" + Math.trunc(random(50));
//     sid = "node" + Math.trunc(random(semanticCloud.semanticNodes.length));
//     createWordNode(newid, "text", sid);
//     setWordNodeText(newid, newid, newid);
//     //updateWordNodePosition(newid, mouseX, mouseY);
//     //setWordNodeWeight(newid,20);
//     wordCloud.moveStep = 10;
//     semanticCloud.moveStep = 10;
// }

function createWordNode(id, type, sid) {
    var node = wordCloud.findNode(id);
    if (node == null) {
        node = new WordNode();
        node.guid = id;
        node.type = type;
        wordCloud.push(node);
        addWordNodeToGroup(id, sid);
    }
    return node;
}
function setWordNodeText(id, cloudText, stemmedText) {
    var node = wordCloud.findNode(id);
    if (node != null && node.type == "text") {
        node.cloudText = cloudText;
        node.stemmedText = stemmedText;
        var tsize = getTextSize(node.cloudText, "" + node.weight + "px Arial", node.weight);
        node.w = tsize.w;
        node.h = tsize.h;
    }
}

function setWordNodeWeight(id, weight) {
    var node = wordCloud.findNode(id);
    if (node != null) {
        node.weight = parseFloat(weight);
        var tsize = getTextSize(node.cloudText, "" + node.weight + "px Arial", node.weight);
        node.w = tsize.w;
        node.h = tsize.h;
    }
}

function updateWordNodePosition(id, xposi, yposi) {
    var node = wordCloud.findNode(id);
    if (node != null) {
        node.x = xposi;
        node.y = yposi;
    }
}

function addWordNodeToGroup(id, snid) {
    var semanticNode = semanticCloud.findNode(snid);
    if (semanticNode != null) {
        var node = wordCloud.findNode(id);
        if (node != null) {
            var previousSNode = node.semanticNode;
            if (previousSNode != null) {
                previousSNode.removeWordNode(node);
            }
            node.semanticNode = semanticNode;
            semanticNode.wordNodes.push(node);
            node.x = semanticNode.x + random(20) - 10;
            node.y = semanticNode.y + random(20) - 10;
        }
    }
}

function addSemanticNode(snid, semantic) {
    var semanticNode = semanticCloud.findNode(snid);
    if (semanticNode == null) {
        var semanticNode = new SemanticNode();
        semanticNode.x = random(windowWidth);
        semanticNode.y = random(windowHeight);
        semanticNode.semantic = semantic;
        semanticNode.guid = snid;
        semanticCloud.push(semanticNode);
    }
}

function connectSemanticNode(snid1, snid2) {
    var firstNode = semanticCloud.findNode(snid1);
    var secondNode = semanticCloud.findNode(snid2);
    if (firstNode != null && secondNode != null) {
        firstNode.connect(secondNode);
        secondNode.connect(firstNode);
    }
}

function setSemanticNodeColor(snid, r, g, b) {
    var semanticNode = semanticCloud.findNode(snid);
    if (semanticNode != null) {
        semanticNode.nodeColor = [parseInt(r), parseInt(g), parseInt(b)];
    }
}

function resetMoveStep() {
    wordCloud.resetMoveStep();
    semanticCloud.resetMoveStep();
}