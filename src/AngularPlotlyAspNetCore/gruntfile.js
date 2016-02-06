module.exports = function (grunt) {

	grunt.loadNpmTasks('grunt-contrib-uglify');
	grunt.loadNpmTasks('grunt-contrib-watch');
	grunt.loadNpmTasks('grunt-contrib-sass');

	grunt.initConfig({
		bower: {
			install: {
				options: {
					targetDir: "wwwroot/lib",
					layout: "byComponent",
					cleanTargetDir: false
				}
			}
		},
		uglify: {
			my_target: {
				files: { 'wwwroot/app.js': ['app/app.js', 'app/**/*.js'] }
			}
		},
		sass: {
		    dist: {
		        files: {
		            'wwwroot/css/main.css': 'css/main.scss'
		        }
		    }
		},
		watch: {
			scripts: {
			    files: ['app/**/*.js', 'wwwroot/css/main.scss'],
				tasks: ['uglify']
			}
		}
	});

	grunt.registerTask("bower", ["bower:install"]);
	grunt.registerTask('default', ['sass', 'uglify', 'watch']);

	grunt.loadNpmTasks("grunt-bower-task");
};