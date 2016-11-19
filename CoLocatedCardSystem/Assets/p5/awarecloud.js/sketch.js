var cloud;
function setup() {
    frameRate(30);
    createCanvas(windowWidth, windowHeight);
    background(255);
    cloud = new WordCloud();
    randomSeed(0);
}

function draw() {
    background(25, 52, 65);
    cloud.update();
    fill(252, 255, 245);
    noStroke();
    for (var i = 0; i < cloud.wordNodes.length; i++) {
        var node = cloud.wordNodes[i];
        if (node.type == 1) {
            image(node.image, node.x + 5, node.y + 5, node.w, node.h);
        }
    }
    for (var i = 0; i < cloud.wordNodes.length; i++) {
        var node = cloud.wordNodes[i];
        if (node.type == 0) {
            noStroke();
            fill(node.textColor[0], node.textColor[1], node.textColor[2]);
            textSize(node.weight);
            text(node.cloudText, node.x + 5, node.y + node.h - node.weight / 3);
            noFill();
            stroke(255);
            strokeWeight(1);
            rect(node.x, node.y, node.w, node.h);
        }
    }
}

function mouseClicked() {//for debug
    updateNode("0",
        "Alex",
        "0,255,255",
        random(["apple", "bear", "peach", "orange", "banana", "grape", "pear"]),
        random(["apple", "bear", "cat", "dog"]),
        random(["red", "blue", "green", "orange", "pink", "yellow"]),
        "" + random(20, 50), "" + mouseX, "" + mouseY, "false");
}

function mouseReleased() {
    //cloud.removeNode("abc", "test");ddd
}

function removeNode(text, group) {
    cloud.removeNode(text, group);
}

function updateNode(type, owner, color, text, stemmedText,group, weight, x, y, hightlight) {
    var node = cloud.findNode(stemmedText, group);
    cloud.step = 10;
    if (node == null) {
        node = new Node();
        node.type = parseInt(type);
        node.cloudText = text;
        node.owner = owner;
        var colors = color.split(",");
        node.textColor = [parseFloat(colors[0]), parseFloat(colors[1]), parseFloat(colors[2])];
        node.stemmedText = stemmedText;
        node.weight = parseInt(weight);
        node.x = parseInt(x);
        node.y = parseInt(y);
        node.attrX = parseInt(x);
        node.attrY = parseInt(y);
        node.hightlight = (hightlight == "true");
        node.group = group;
        if (type == "0") {
            var font = node.weight + "px Arial";
            var tsize = getTextSize(node.cloudText, font, node.weight);
            node.w = tsize.w;
            node.h = tsize.h;
        }
        else if (type == "1") {
            var img = new Image();
            img.src = "../../review/" + node.cloudText;
            node.image = loadImage("../../review/" + node.cloudText);
            node.w = node.weight * img.width / 200 + 10;
            node.h = node.weight * img.height / 200 + 10;
        }
        cloud.push(node);
    } else {
        node.attrX = parseInt(x);
        node.attrY = parseInt(y);
    }
}

