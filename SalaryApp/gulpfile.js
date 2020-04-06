/// <binding ProjectOpened='watch' />
const gulp = require('gulp'); // Gulp 3 docs: https://github.com/gulpjs/gulp/blob/v3.9.1/docs/API.md
const less = require('gulp-less'); // does not support gulp 4. https://www.npmjs.com/package/gulp-less
const crassMinifier = require('gulp-crass-minifier');
const rename = require('gulp-rename');
const terserMinifier = require('gulp-minify'); // https://www.npmjs.com/package/gulp-minify
//const del = require('del');

const lessFiles = 'wwwroot/*.less';
// TODO glob negation is deprecated in version 5. May need to use "ignore" option. https://github.com/isaacs/node-glob#comments-and-negation
const cssFiles = ['wwwroot/*.css', '!*/*.min.css'];
const jsFiles = ['wwwroot/*.js', '!*/*.min.js'];

const tnCompile = 'compile';
gulp.task(tnCompile, compile);

const tnMinify = 'minify';
gulp.task(tnMinify, minify);

const tnCompileAndMinify = 'compile-and-minify';
gulp.task(tnCompileAndMinify, [tnCompile], minify);

const tnMinifyJS = 'minify-JS';
gulp.task(tnMinifyJS, minifyJS);

const tnWatch = 'watch';
gulp.task(tnWatch, [tnCompileAndMinify, tnMinifyJS], function (asyncCallback) {
    //gulp.watch(lessFiles, [tnCompileAndMinify]);
    gulp.watch(lessFiles, [tnCompile]);
    gulp.watch(cssFiles, [tnMinify]);
    gulp.watch(jsFiles, [tnMinifyJS]);
    asyncCallback();
});

function writeToBase() {
    return gulp.dest(function (file) {
        return file.base;
    });
}

function compile() {
    return gulp.src(lessFiles)
        .pipe(less())
        .pipe(writeToBase());
}

function minify() {
    return gulp.src(cssFiles)
        .pipe(crassMinifier({
            pretty: false,
            optimize: true,
            o1: true,
            css4: false,
            browser_min: {
                // Earliest releases in 2019
                chrome: 72,
                firefox: 65,
                opera: 58,
            }
        }))
        .pipe(rename(function (path) {
            path.extname = ".min.css";
        }))
        .pipe(writeToBase());
}

function minifyJS() {
    return gulp.src(jsFiles)
        .pipe(terserMinifier({
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(writeToBase());
}

//const tnClean = 'clean';
//gulp.task(tnClean, function (asyncCallback) {
//    del(...);
//    asyncCallback();
//});

//gulp.task('rebuild', [tnClean, tnCompile, tnMinify]);
