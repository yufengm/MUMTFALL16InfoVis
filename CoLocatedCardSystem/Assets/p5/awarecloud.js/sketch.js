var cloud;
var debugCounter = 0;
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
        if (node.type == "image") {
            image(node.image, node.x + 5, node.y + 5, node.w, node.h);
        }
    }

    for (var i = 0; i < cloud.wordNodes.length; i++) {
        var node = cloud.wordNodes[i];
        if (node.type == "point") {
            stroke(255, 255, 0);
            fill(node.textColor[0], node.textColor[1], node.textColor[2]);
            ellipse(node.x + node.w / 2, node.y + node.w / 2, node.weight, node.weight);
        }
    }

    for (var i = 0; i < cloud.wordNodes.length; i++) {
        var node = cloud.wordNodes[i];
        if (node.type == "text") {
            noStroke();
            fill(node.textColor[0], node.textColor[1], node.textColor[2]);
            textSize(node.weight);
            text(node.cloudText, node.x + 5, node.y + node.h - 2);
            // noFill();
            // stroke(255);
            // strokeWeight(1);
            // rect(node.x, node.y, node.w, node.h);
        }
    }
}

function mouseClicked() {//for debug
    if (debugCounter < 100) {
        updateNode("point",
            "Alex",
            "255,0,0",
            random(["apple", "bear", "peach", "orange", "banana", "grape", "pear"]),
            random(100000),
            random(["red", "blue", "green", "orange", "pink", "yellow"]),
            "" + 10, "" + mouseX, "" + mouseY, "false");
    } else {
        updateNode("text",
            "Alex",
            "254,255,252",
            random(["apple", "bear", "peach", "orange", "banana", "grape", "pear"]),
            random(100000),
            random(["red", "blue", "green", "orange", "pink", "yellow"]),
            "" + 10, "" + mouseX, "" + mouseY, "false");
    }
    debugCounter++;
    cloud.moveStep=1;
}

function mouseReleased() {
    //cloud.removeNode("abc", "test");ddd
}

function removeNode(text, group) {
    cloud.removeNode(text, group);
}

function updateNode(type, owner, color, text, stemmedText, group, weight, x, y, hightlight) {
    var node = cloud.findNode(stemmedText, group);
    if (node == null) {
        node = new Node();
        node.type = type;
        node.cloudText = text;
        node.owner = owner;
        var colors = color.split(",");
        node.textColor = [parseFloat(colors[0]), parseFloat(colors[1]), parseFloat(colors[2])];
        node.stemmedText = stemmedText;
        node.weight = parseInt(weight);
        node.x = parseInt(x);
        node.y = parseInt(y);
        node.attrX = windowWidth / 2;
        node.attrY = windowHeight / 2;
        node.hightlight = (hightlight == "true");
        node.group = group;
        if (type == "text") {
            var font = node.weight + "px Arial";
            var tSize = getTextSize(node.cloudText, font, node.weight);
            node.w = tSize.w;
            node.h = tSize.h;
        }
        else if (type == "image") {
            var img = new Image();
            img.src = "../../review/" + node.cloudText;
            node.image = loadImage("../../review/" + node.cloudText);
            node.w = node.weight * img.width / 200 + 10;
            node.h = node.weight * img.height / 200 + 10;
        } else if (type == "point") {
            node.w = node.weight;
            node.h = node.weight;
        }
        cloud.push(node);
    } else {
        node.attrX = parseInt(x);
        node.attrY = parseInt(y);
    }
}

