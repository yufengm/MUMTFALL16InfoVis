var cloud;
function setup() {
    frameRate(30);
    createCanvas(windowWidth, windowHeight);
    background(255);
    cloud = new WordCloud();
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
            textSize(node.weight);
            text(node.txt, node.x + 5, node.y + node.h + 5);
        }
    }
}

function mousePressed() {//for debug
    updateNode("1", "0bb884e7-0e79-383f-9173-2f23799c41f0.jpg", "abc", "test", "" + random(20, 50), "" + mouseX, "" + mouseY, "false");

}

function mouseReleased() {
    cloud.removeNode("abc", "test");
}

function removeNode(text, group) {
    cloud.removeNode(text, group);
}

function updateNode(type, text, stemmedText, group, weight, x, y, hightlight) {
    var node = cloud.findNode(stemmedText, group);
    cloud.step = 10;
    if (node == null) {
        node = new Node();
        node.type = parseInt(type);
        node.txt = text;
        node.stemmedText = stemmedText;
        //node.weight = parseInt(weight);
        node.weight=random(15, 25);//for debug
        node.x = parseInt(x);
        node.y = parseInt(y);
        node.attrX = parseInt(x);
        node.attrY = parseInt(y);
        node.hightlight = (hightlight == "true");
        node.group = group;
        if (type == "0") {
            var font = node.weight + "px Arial";
            var tsize = getTextSize(node.txt, font, node.weight);
            node.w = tsize.w;
            node.h = tsize.h;
        }
        else if (type == "1") {
            var img = new Image();
            img.src = "../../review/" + node.txt;
            node.image = loadImage("../../review/" + node.txt);
            node.w = node.weight * img.width / 200+10;
            node.h = node.weight * img.height / 200+10;
        }
        cloud.push(node);
    } else {
        node.attrX = parseInt(x);
        node.attrY = parseInt(y);
    }
}

