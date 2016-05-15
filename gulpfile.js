﻿/// <binding BeforeBuild='scripts' ProjectOpened='watch' />
// include plug-ins
var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');
var insert = require('gulp-insert');



var config = {
    //Include all js files but exclude any min.js files
    src: ['assets/js/rdash/**/*.js', '!/assets/js/rdash/**/*.min.js']
}

//delete the output file(s)
gulp.task('clean', function () {
    //del is an async function and not a gulp plugin (just standard nodejs)
    //It returns a promise, so make sure you return that from this task function
    //  so gulp knows when the delete is complete
    return del(['assets/js/rdash/rdash.min.js']);
});

// Combine and minify all files from the app folder
// This tasks depends on the clean task which means gulp will ensure that the 
// Clean task is completed before running the scripts task.
gulp.task('scripts', ['clean'], function () {

    return gulp.src(config.src)
     // .pipe(uglify())
       
      .pipe(concat('rdash.min.js'))
      .pipe(insert.prepend('//This file is generated by gulp any changes will be deleted !!'))
      .pipe(gulp.dest('assets/js/rdash'));
});

gulp.task('text',function(){

  return gulp.src('assets/js/rdash/rdash.min.js')
  

});
gulp.task('watch', function () {
    return gulp.watch(config.src, ['scripts']);
});
//Set a default tasks
gulp.task('default', ['scripts'], function () { });