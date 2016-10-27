var cloudArray=[];

function setup() {
    frameRate(30);
    createCanvas(windowWidth, windowHeight);
    background(0);
    for (var i = 0; i < 3; i++) {
        var cloud = new WordCloud();
        for (var j = 0; j < 5; j++) {
            var node = new Node();
            node.txt = "test" + j;
            node.x = windowWidth * (i + 1) /4 +random(-5, 5);
            node.y = windowHeight * (i + 1) / 4 + random(-5, 5);
            node.id = i * 5 + j;
            cloud.push(node);
        }
        for (var m = 0; m < cloud.wordNodes.length; m++) {
            for (var n = 0; n < cloud.wordNodes.length; n++) {
                if (m != n) {
                    cloud.wordNodes[m].connections.push(cloud.wordNodes[n]);
                }
            }
        }
        append(cloudArray, cloud);
    }
}

function draw() {
    background(0);
    fill(255, 100, 0);
    for (var i = 0; i < cloudArray.length; i++) {
        cloudArray[i].update();
        for (j = 0; j < cloudArray[i].wordNodes.length; j++) {
            var node = cloudArray[i].wordNodes[j];
            textSize(node.textSize);
            text(node.txt, node.x, node.y);
        }
    }
}

function addNode(id, text) {
    var node = new Node();
    node.txt = text;
    node.x = windowWidth / 2 + random(-5, 5);
    node.y = windowHeight / 2 + random(-5, 5);
    node.connections.push(cloud.wordNodes[0]);
    node.id = id;
    cloud.push(node);
}

function distance(node1, node2) {
    var result = Math.sqrt(Math.pow(node1.x - node2.x, 2) + Math.pow(node1.y - node2.y, 2));
    if (result == 0) {
        return 0.1;
    }
    return result;
}

