var cloud;
function setup() {
    frameRate(30);
    createCanvas(windowWidth, windowHeight);
    background(255);
    cloud = new WordCloud();
}

function draw() {
    background(25,52,65);
    cloud.update();
    for (var i = 0; i < cloud.wordNodes.length; i++) {
        fill(252,255,245);
        noStroke();
        var node = cloud.wordNodes[i];
        textSize(node.weight);
        text(node.txt, node.x + 3, node.y + node.h + 3);
    }
}

function mousePressed() {//for debug
    addNode("0", "abc" + Math.floor(random(100)), "" + random(20, 50), "" + mouseX, "" + mouseY, "false", "test");
}

function addNode(type, text, weight, x, y, hightlight, group) {
    var node = cloud.findNode(text, group);
    cloud.step = 10;
    if (node == null) {
        node = new Node();
        node.type = parseInt(type);
        node.txt = text;
        node.weight = parseInt(weight);
        node.x = parseInt(x);
        node.y = parseInt(y);
        node.attrX = parseInt(x);
        node.attrY = parseInt(y);
        node.hightlight = (hightlight == "true");
        node.group = group;
        var font = node.weight + "px Arial";
        var tsize = getTextSize(node.txt, font, node.weight);
        node.w = tsize.w;
        node.h = tsize.h;
        cloud.push(node);
    } else {
        node.attrX = parseInt(x);
        node.attrY = parseInt(y);
    }
}

