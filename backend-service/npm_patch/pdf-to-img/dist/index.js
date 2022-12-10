"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function (o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
        desc = { enumerable: true, get: function () { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function (o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function (o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function (o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.pdf = void 0;
require("./polyfill"); // do this before pdfjs
// 🛑 inspite of esModuleInterop being on, you still need to use `import *`, and there are no typedefs
const _pdfjs = __importStar(require("pdfjs-dist/legacy/build/pdf"));
const canvasFactory_1 = require("./canvasFactory");
const parseInput_1 = require("./parseInput");
const pdfjs = _pdfjs;
/** required since k-yle/pdf-to-img#58, the objects from pdfjs are weirdly structured */
const sanitize = (x) => {
    const obj = JSON.parse(JSON.stringify(x));
    // remove UTF16 BOM and weird 0x0 character introduced in k-yle/pdf-to-img#138 and k-yle/pdf-to-img#184
    for (const key in obj) {
        if (typeof obj[key] === "string") {
            // eslint-disable-next-line no-control-regex -- this is deliberate
            obj[key] = obj[key].replace(/(^þÿ|\x00)/g, "");
        }
    }
    return obj;
};
/**
 * Converts a PDF to a series of images. This returns a `Symbol.asyncIterator`
 *
 * @param input Either (a) the path to a pdf file, or (b) a data url, or (c) a buffer, or (d) a ReadableStream.
 *
 * @example
 * ```js
 * import pdf from "pdf-to-img";
 *
 * for await (const page of await pdf("example.pdf")) {
 *   expect(page).toMatchImageSnapshot();
 * }
 *
 * // or if you want access to more details:
 *
 * const doc = await pdf("example.pdf");
 * expect(doc.length).toBe(1);
 * expect(doc.metadata).toEqual({ ... });
 *
 * for await (const page of doc) {
 *   expect(page).toMatchImageSnapshot();
 * }
 * ```
 */
function pdf(input, options = {}) {
    return __awaiter(this, void 0, void 0, function* () {
        const data = yield (0, parseInput_1.parseInput)(input);
        const { docInitParams } = options;
        const pdfDocument = yield pdfjs.getDocument(Object.assign(Object.assign({
            password: options.password, cMapUrl: `${process.cwd()}/node_modules/pdfjs-dist/cmaps/`, cMapPacked: true, standardFontDataUrl: `${process.cwd()}/node_modules/pdfjs-dist/standard_fonts/`
        }, docInitParams), { data })).promise;
        return {
            length: pdfDocument.numPages,
            metadata: sanitize((yield pdfDocument.getMetadata()).info),
            [Symbol.asyncIterator]() {
                return {
                    pg: 0,
                    next() {
                        var _a;
                        return __awaiter(this, void 0, void 0, function* () {
                            // eslint-disable-next-line no-unreachable-loop -- broken rule, this is a generator
                            while (this.pg < pdfDocument.numPages) {
                                this.pg += 1;
                                const page = yield pdfDocument.getPage(this.pg);
                                const viewport = page.getViewport({ scale: (_a = options.scale) !== null && _a !== void 0 ? _a : 1 });
                                const canvasFactory = new canvasFactory_1.NodeCanvasFactory();
                                const { canvas, context } = canvasFactory.create(viewport.width, viewport.height);
                                yield page.render({
                                    canvasContext: context,
                                    viewport,
                                    canvasFactory,
                                }).promise;
                                return { done: false, value: canvas.toBuffer() };
                            }
                            return { done: true, value: undefined };
                        });
                    },
                };
            },
        };
    });
}
exports.pdf = pdf;
