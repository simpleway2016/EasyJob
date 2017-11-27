declare var AllSelectedControls: EditorControl[];
declare var editor: Editor;
declare var ManyPointDefined: number;
declare function documentElementMouseDown(e: MouseEvent): void;
declare class EditorControl {
    container: IEditorControlContainer;
    propertyDialog: PropertyDialog;
    ctrlKey: boolean;
    isInGroup: boolean;
    isDesignMode: boolean;
    element: any;
    _selected: boolean;
    _moveAllSelectedControl: boolean;
    selected: boolean;
    rect: any;
    _id: string;
    id: string;
    private mouseDownX;
    private mouseDownY;
    private undoMoveObj;
    constructor(element: any);
    getPropertiesCaption(): string[];
    getProperties(): string[];
    run(): void;
    onDevicePointValueChanged(devPoint: any): void;
    getJson(): {
        rect: any;
        constructorName: any;
    };
    getScript(): string;
    isIntersectWith(rect: any): boolean;
    isIntersect(rect1: any, rect: any): boolean;
    showProperty(): void;
    onSelectedChange(): void;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
}
declare class LineControl extends EditorControl {
    lineElement: SVGLineElement;
    virtualLineElement: SVGLineElement;
    pointEles: SVGCircleElement[];
    moving: boolean;
    startX: number;
    startY: number;
    valueX: any;
    valueY: any;
    rect: {
        x: number;
        y: number;
        width: number;
        height: number;
    };
    point: any;
    getJson(): any;
    lineWidth: string;
    color: string;
    constructor();
    getPropertiesCaption(): string[];
    getProperties(): string[];
    isIntersectWith(rect: any): boolean;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
    onSelectedChange(): void;
    resetPointLocation(): void;
    setEvent(pointEle: any, xName: any, yName: any): void;
    private undoObj;
    pointMouseDown(e: MouseEvent, pointEle: any, xName: string, yName: string): void;
    pointMouseMove(e: MouseEvent, pointEle: any, xName: string, yName: string): void;
    pointMouseUp(e: MouseEvent, pointEle: any): void;
}
declare class RectControl extends EditorControl {
    rectElement: SVGGraphicsElement;
    pRightBottom: SVGCircleElement;
    moving: boolean;
    startX: number;
    startY: number;
    rect: any;
    strokeWidth: string;
    colorStroke: string;
    colorFill: string;
    _devicePoint: string;
    devicePoint: string;
    _scriptOnValueChange: string;
    scriptOnValueChange: string;
    onDevicePointValueChanged(devPoint: any): void;
    getPropertiesCaption(): string[];
    getProperties(): string[];
    constructor(element: any);
    isIntersectWith(rect: any): boolean;
    onSelectedChange(): void;
    resetPointLocation(): void;
    setEvent(pointEle: any): void;
    private undoObj;
    pointMouseDown(e: MouseEvent, pointEle: any): void;
    pointMouseMove(e: MouseEvent, pointEle: any): void;
    pointMouseUp(e: MouseEvent, pointEle: any): void;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
}
declare class EllipseControl extends EditorControl {
    rootElement: SVGEllipseElement;
    pointEles: SVGCircleElement[];
    moving: boolean;
    startX: number;
    startY: number;
    rect: any;
    strokeWidth: string;
    colorStroke: string;
    colorFill: string;
    _devicePoint: string;
    devicePoint: string;
    _scriptOnValueChange: string;
    scriptOnValueChange: string;
    onDevicePointValueChanged(devPoint: any): void;
    getPropertiesCaption(): string[];
    getProperties(): string[];
    constructor();
    isIntersectWith(rect: any): boolean;
    onSelectedChange(): void;
    resetPointLocation(): void;
    setEvent(pointEle: any): void;
    private undoObj;
    pointMouseDown(e: MouseEvent, pointEle: any): void;
    pointMouseMove(e: MouseEvent, pointEle: any): void;
    pointMouseUp(e: MouseEvent, pointEle: any): void;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
}
declare class CircleControl extends EditorControl {
    rootElement: SVGCircleElement;
    pointEles: SVGCircleElement[];
    moving: boolean;
    startX: number;
    startY: number;
    rect: any;
    strokeWidth: string;
    colorStroke: string;
    colorFill: string;
    _devicePoint: string;
    devicePoint: string;
    _scriptOnValueChange: string;
    scriptOnValueChange: string;
    onDevicePointValueChanged(devPoint: any): void;
    getPropertiesCaption(): string[];
    getProperties(): string[];
    constructor();
    isIntersectWith(rect: any): boolean;
    onSelectedChange(): void;
    resetPointLocation(): void;
    setEvent(pointEle: any): void;
    private undoObj;
    pointMouseDown(e: MouseEvent, pointEle: any): void;
    pointMouseMove(e: MouseEvent, pointEle: any): void;
    pointMouseUp(e: MouseEvent, pointEle: any): void;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
}
declare class ImageControl extends RectControl {
    imgElement: SVGImageElement;
    imgSrc: string;
    getPropertiesCaption(): string[];
    getProperties(): string[];
    constructor();
}
declare class TextControl extends RectControl {
    textElement: SVGTextElement;
    selectingElement: SVGRectElement;
    text: string;
    size: number;
    colorFill: string;
    _canSetValue: boolean;
    canSetValue: boolean;
    _devicePoint: string;
    _lastDevPoint: any;
    devicePoint: string;
    onDevicePointValueChanged(devPoint: any): void;
    run(): void;
    getPropertiesCaption(): string[];
    getProperties(): string[];
    rect: any;
    constructor();
    onSelectedChange(): void;
    resetPointLocation(): void;
}
declare class CylinderControl extends EditorControl {
    private _value;
    private _max;
    private _min;
    value: any;
    max: any;
    min: any;
    _devicePoint: string;
    devicePoint: string;
    onDevicePointValueChanged(devPoint: any): void;
    rectElement: SVGRectElement;
    cylinderElement: SVGRectElement;
    pRightBottom: SVGCircleElement;
    moving: boolean;
    startX: number;
    startY: number;
    rect: any;
    strokeWidth: string;
    colorStroke: string;
    colorFill: string;
    colorBg: string;
    getPropertiesCaption(): string[];
    getProperties(): string[];
    constructor();
    resetCylinder(rect: any): void;
    isIntersectWith(rect: any): boolean;
    onSelectedChange(): void;
    resetPointLocation(): void;
    setEvent(pointEle: any): void;
    private undoObj;
    pointMouseDown(e: MouseEvent, pointEle: any): void;
    pointMouseMove(e: MouseEvent, pointEle: any): void;
    pointMouseUp(e: MouseEvent, pointEle: any): void;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
}
declare class TrendControl extends EditorControl {
    rectElement: SVGRectElement;
    pRightBottom: SVGCircleElement;
    line_left_Ele: SVGLineElement;
    line_bottom_Ele: SVGLineElement;
    pathElement1: SVGPathElement;
    pathElement2: SVGPathElement;
    pathElement3: SVGPathElement;
    pathElement4: SVGPathElement;
    pathElement5: SVGPathElement;
    pathElement6: SVGPathElement;
    pathElement7: SVGPathElement;
    pathElement8: SVGPathElement;
    pathElement9: SVGPathElement;
    pathElement10: SVGPathElement;
    pathElement11: SVGPathElement;
    pathElement12: SVGPathElement;
    private _max;
    private _min;
    values1: any[];
    values2: any[];
    values3: any[];
    values4: any[];
    values5: any[];
    values6: any[];
    values7: any[];
    values8: any[];
    values9: any[];
    values10: any[];
    values11: any[];
    values12: any[];
    private _value1;
    value1: any;
    private _value2;
    value2: any;
    private _value3;
    value3: any;
    private _value4;
    value4: any;
    private _value5;
    value5: any;
    private _value6;
    value6: any;
    private _value7;
    value7: any;
    private _value8;
    value8: any;
    private _value9;
    value9: any;
    private _value10;
    value10: any;
    private _value11;
    value11: any;
    private _value12;
    value12: any;
    max: any;
    min: any;
    _devicePoint1: string;
    devicePoint1: string;
    _devicePoint2: string;
    devicePoint2: string;
    _devicePoint3: string;
    devicePoint3: string;
    _devicePoint4: string;
    devicePoint4: string;
    _devicePoint5: string;
    devicePoint5: string;
    _devicePoint6: string;
    devicePoint6: string;
    _devicePoint7: string;
    devicePoint7: string;
    _devicePoint8: string;
    devicePoint8: string;
    _devicePoint9: string;
    devicePoint9: string;
    _devicePoint10: string;
    devicePoint10: string;
    _devicePoint11: string;
    devicePoint11: string;
    _devicePoint12: string;
    devicePoint12: string;
    onDevicePointValueChanged(devPoint: any): void;
    running: boolean;
    moving: boolean;
    startX: number;
    startY: number;
    rect: any;
    colorFill: string;
    colorLineLeftBottom: string;
    colorLine1: string;
    colorLine2: string;
    colorLine3: string;
    colorLine4: string;
    colorLine5: string;
    colorLine6: string;
    colorLine7: string;
    colorLine8: string;
    colorLine9: string;
    colorLine10: string;
    colorLine11: string;
    colorLine12: string;
    getPropertiesCaption(): string[];
    getProperties(): string[];
    constructor();
    isIntersectWith(rect: any): boolean;
    onSelectedChange(): void;
    run(): void;
    reDrawTrend(): void;
    resetPointLocation(): void;
    setEvent(pointEle: any): void;
    private undoObj;
    pointMouseDown(e: MouseEvent, pointEle: any): void;
    pointMouseMove(e: MouseEvent, pointEle: any): void;
    pointMouseUp(e: MouseEvent, pointEle: any): void;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
}
declare class ButtonAreaControl extends EditorControl {
    rectElement: SVGGraphicsElement;
    pRightBottom: SVGCircleElement;
    moving: boolean;
    startX: number;
    startY: number;
    rect: any;
    getPropertiesCaption(): string[];
    getProperties(): string[];
    constructor();
    run(): void;
    isIntersectWith(rect: any): boolean;
    onSelectedChange(): void;
    resetPointLocation(): void;
    setEvent(pointEle: any): void;
    private undoObj;
    pointMouseDown(e: MouseEvent, pointEle: any): void;
    pointMouseMove(e: MouseEvent, pointEle: any): void;
    pointMouseUp(e: MouseEvent, pointEle: any): void;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
}
declare class GroupControl extends EditorControl implements IEditorControlContainer {
    controls: any[];
    removeControl(ctrl: EditorControl): void;
    addControl(ctrl: EditorControl): void;
    groupElement: SVGGElement;
    pRightBottom: SVGCircleElement;
    moving: boolean;
    startX: number;
    startY: number;
    virtualRectElement: SVGRectElement;
    contentWidth: number;
    contentHeight: number;
    private lastRect;
    rect: any;
    getPropertiesCaption(): string[];
    getProperties(): string[];
    constructor(element: any);
    isIntersectWith(rect: any): boolean;
    onSelectedChange(): void;
    resetPointLocation(): void;
    setEvent(pointEle: any): void;
    private undoObj;
    pointMouseDown(e: MouseEvent, pointEle: any): void;
    pointMouseMove(e: MouseEvent, pointEle: any): void;
    pointMouseUp(e: MouseEvent, pointEle: any): void;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
}
